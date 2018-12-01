import { ProductListModel, VariantModel } from '../products/product';

export class StockAllocationRequest {
    gIWDName: string;
    gIWDDescription: string;
    fromLocationId: string;
    toLocationId: string;
    allocationProducts: Array<StockAllocationProductModel>;
}

export class StockTypeModel {
    id: string;
    name: string;
    code: string;
    description: string;
}

export class StockAllocationProductModel {
    variantId: string;
    stockTypeId: string;
    quantity: number;
}

export class StockAllocationProductViewModel {
    product: ProductListModel;
    variants: VariantModel[];
    isVariantsLoading: boolean;
    variant: VariantViewModel;
    stockType: StockTypeModel;
    quantity: number;
}

export class VariantViewModel {
    id: string;
    name: string;
}
