import { Component, ViewEncapsulation } from '@angular/core';
import { ActionType } from 'src/app/shared/constant/action-type.constant';
import { Store, select } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';
import { Button } from 'src/app/shared/base-model/button.model';
import * as fromProduct from '../products/state/product.reducer';
import * as productActions from '../products/state/product.actions';
import * as productSelector from '../products/state/index';
import * as fromAuths from '../../shared/components/auth/state/index';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { UserDefinedColumnSetting } from 'src/app/shared/base-model/user-defined-column-setting.model';
import { PagingFilterCriteria } from 'src/app/shared/base-model/paging-filter-criteria';
import { ProductAddComponent } from './product-add/product-add.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { ProductListModel } from './product';

const AddFormIndex = 1;

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductComponent extends ComponentBase {
  constructor(private store: Store<fromProduct.ProductState>) {
    super();
  }
  public addSuccessMessage = 'Product is added.';
  public updateSuccessMessage = 'Product is updated.';
  public deleteSuccessMessage = 'Product is deleted.';
  public datasource: Array<ProductListModel> = [];
  public listButton = Array<Button>();
  public title;
  public pageSize = 10;
  public actionType = ActionType.dialog;
  componentActive = true;
  isHiddenSearchBox = false;
  userDefinedColumnSetting: UserDefinedColumnSetting;
  public queryText = '';
  onInit() {
    this.title = 'Product Management';
    this.handleSubscription(
      this.store
        .pipe(
          select(fromAuths.getUserId),
          takeWhile(() => this.componentActive)
        )
        .subscribe((id: string) => {
          if (id == null) {
            return;
          }
          this.userDefinedColumnSetting = new UserDefinedColumnSetting(
            `${id}_UserDefinedColumnsProduct`,
            'name,description'
          );
          this.getProducts(this.queryText);
          this.setButtonsConfiguration();
        })
    );

    this.handleSubscription(
      this.store
        .pipe(
          select(productSelector.getSelectedItem),
          takeWhile(() => this.componentActive)
        )
        .subscribe((id: string | null) => {
          if (id) {
            this.changeListButton(false);
          } else {
            this.changeListButton(true);
          }
        })
    );
  }

  onDestroy() { }

  getProducts(searchText: string) {
    this.store.dispatch(new productActions.GetProducts(new PagingFilterCriteria(1, this.pageSize), searchText));
    this.store.pipe(select(productSelector.getProducts),
      takeWhile(() => this.componentActive))
      .subscribe((products: Array<ProductListModel>) => {
        this.datasource = products;
      });
  }

  changeSelectedPage(page: number) {
    this.store.dispatch(
      new productActions.GetProducts(
        new PagingFilterCriteria(page + 1, this.pageSize), this.queryText
      )
    );
  }

  setButtonsConfiguration() {
    this.listButton = [
      new Button({
        id: AddFormIndex,
        title: 'Add',
        component: ProductAddComponent,
        configDialog: { size: 'lg', centered: true, backdrop: 'static' },
        disable: false
      }),
      new Button({
        id: AddFormIndex,
        title: 'Edit',
        component: ProductEditComponent,
        configDialog: { size: 'lg', centered: true, backdrop: 'static' },
        disable: true
      })
      // new Button({
      //     id: 2,
      //     title: 'Delete',
      //     component: DeleteFieldComponent,
      //     configDialog: { size: 'lg', centered: true, backdrop: 'static' },
      //     disable: true
      // })
    ];
  }

  changeListButton(isDisabled: boolean) {
    if (!this.listButton) {
      return;
    }
    this.listButton.forEach(btn => {
      if (btn.title === 'Edit' || btn.title === 'Delete') {
        btn.disable = isDisabled;
      }
    });
  }

  eventSearchQuery(event: any) {
    this.queryText = event;
    this.getProducts(this.queryText);
  }
}
