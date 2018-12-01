import { Component, ViewEncapsulation } from '@angular/core';
import { LocationModel, LocationType } from '../locations/location.model';
import {
  StockAllocationProductViewModel,
  StockTypeModel,
  StockAllocationRequest,
  StockAllocationProductModel
} from './allocation.model';
import { ProductListModel, VariantModel } from '../products/product';
import { ProductService } from 'src/app/shared/services/product.service';
import { LocationService } from 'src/app/shared/services/location.service';
import { StockTypeService } from 'src/app/shared/services/stock-type.service';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { StockAllocationService } from 'src/app/shared/services/allocation.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

const wareHouseType = LocationType.wareHouse;
@Component({
  selector: 'app-allocations',
  templateUrl: './allocations.component.html',
  styleUrls: ['./allocations.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AllocationsComponent extends ComponentBase {
  public locations: Array<LocationModel> = [];
  public stockTypes: Array<StockTypeModel> = [];
  public products: Array<ProductListModel> = [];
  public allocationProducts: Array<StockAllocationProductViewModel> = [];

  public isLocationsLoading = true;
  public isProductsLoading = true;
  public isStockTypesLoading = true;

  public selectedLocation = null;

  constructor(
    private productService: ProductService,
    private locationService: LocationService,
    private stockTypeService: StockTypeService,
    private allocationService: StockAllocationService,
    private notificationService: NotificationService) {
    super();
  }

  onInit() {
    this.getLocations();
    this.getProducts();
    this.getStockTypes();
  }

  onDestroy() {

  }

  public isEnableAddButton() {
    return this.selectedLocation ? false : true;
  }

  public isEnableAllocateButton() {
    return (this.allocationProducts.length > 0 && this.checkAvailableToAllocate()) ? false : true;
  }

  private checkAvailableToAllocate() {
    let isAvailable = true;
    this.allocationProducts.forEach(element => {
      if (!element.product
        || !element.variant
        || !element.stockType) {
        isAvailable = false;
      }
    });

    return isAvailable;
  }

  public onClickAllocateButton() {
    const request = this.convertDataRequest();
    this.allocationService.add(request).subscribe(res => {
      this.notificationService.success('Successful');
      this.clearPage();
    });
  }

  public onClickAddButton() {
    const allocationProduct: StockAllocationProductViewModel = {
      product: null,
      variants: [],
      isVariantsLoading: false,
      variant: null,
      stockType: null,
      quantity: 0
    };
    this.allocationProducts.push(allocationProduct);
  }

  private convertDataRequest() {
    const _allocationProducts: Array<StockAllocationProductModel> = [];

    this.allocationProducts.forEach(element => {
      const item: StockAllocationProductModel = {
        variantId: element.variant.id,
        stockTypeId: element.stockType.id,
        quantity: element.quantity
      };
      _allocationProducts.push(item);
    });

    const data: StockAllocationRequest = {
      gIWDName: '',
      gIWDDescription: '',
      fromLocationId: '508c1b19-5e5d-440b-9414-42f7de8879d4',
      toLocationId: this.selectedLocation.id,
      allocationProducts: _allocationProducts
    };

    return data;
  }

  private getLocations() {
    this.isLocationsLoading = true;
    this.locationService.getByType(wareHouseType).subscribe(res => {
      this.locations = res;
      this.isLocationsLoading = false;
    });
  }

  private getProducts() {
    this.isProductsLoading = true;
    this.productService.getAllWithoutPaging().subscribe(res => {
      this.products = res;
      this.isProductsLoading = false;
    });
  }

  private getStockTypes() {
    this.isStockTypesLoading = true;
    this.stockTypeService.getAllWithoutPaging().subscribe(res => {
      this.stockTypes = res;
      this.isStockTypesLoading = false;
    });
  }

  public onSelectedProductChange(index: number) {
    this.clearAllocationProduct(index);
    const productId = this.allocationProducts[index].product.id;
    if (productId) {
      this.allocationProducts[index].isVariantsLoading = true;

      this.productService.getById(productId).subscribe(res => {
        const variants = this.convertVariants(res.variants);
        this.allocationProducts[index].variants = variants;
        this.allocationProducts[index].isVariantsLoading = false;
      });
    }
  }

  private clearAllocationProduct(index: number) {
    this.allocationProducts[index].variants = [];
    this.allocationProducts[index].variant = null;
    this.allocationProducts[index].stockType = null;
    this.allocationProducts[index].quantity = 0;
  }

  private convertVariants(variants: Array<VariantModel>) {
    variants.forEach(item => {
      item.name = this.generatedVariantName(item);
    });
    return variants;
  }

  private generatedVariantName(variant: VariantModel) {
    let name = '';
    variant.fieldValues.forEach(item => {
      name += `${item.name}: `;
      item.value.forEach(element => {
        name += `${element}, `;
      });
      name += `; `;
    });

    return name;
  }

  public onSelectedProductVariantChange(index: number, item: any) {
    this.allocationProducts[index].stockType = null;
    this.allocationProducts[index].quantity = 0;
    if (item) {
      this.allocationProducts[index].variant = {
        id: item.id,
        name: item.name
      };
    }
  }

  public onSelectedLocationChange() {
    this.allocationProducts = [];
  }

  private clearPage() {
    this.allocationProducts = [];
    this.selectedLocation = null;
  }

  public onCheckNumberQuantityChange(index: number) {
    if (this.allocationProducts[index].quantity < 0) {
      this.allocationProducts[index].quantity = 0;
    }
  }
}
