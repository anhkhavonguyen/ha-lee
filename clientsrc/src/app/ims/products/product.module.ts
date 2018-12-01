import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { StoreModule, ActionReducerMap } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { ProductEffects } from './state/product.effect';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { ProductAddComponent } from './product-add/product-add.component';
import { ProductComponent } from './product.component';
import { FieldSharedModule } from '../fields/field.module.shared';
import {
  reducer as assortmentReducer,
  key as productKey,
  ProductState
} from './state/product.reducer';
import {
  reducer as listViewManagementReducer,
  key as listViewManagementKey,
  ListViewManagementState
} from 'src/app/shared/components/list-view-management/state/list-view-management.reducer';
import { NgbModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { VariantsCollectionComponent } from './product-add/variants-collection/variants-collection.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

const homeRoutes: Routes = [{ path: '', component: ProductComponent }];

export interface IProductState {
  products_reducer: ProductState;
  listviewmanagement_reducer: ListViewManagementState;
}

export const reducers: ActionReducerMap<IProductState> = {
  products_reducer: assortmentReducer,
  listviewmanagement_reducer: listViewManagementReducer
};

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    FieldSharedModule,
    RouterModule.forChild(homeRoutes),
    StoreModule.forFeature(`products`, reducers),
    EffectsModule.forFeature([ProductEffects]),
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    NgbModule.forRoot(),
    NgbDropdownModule.forRoot(),
    NgxDatatableModule
  ],
  declarations: [
    ProductComponent,
    ProductAddComponent,
    ProductEditComponent,
    VariantsCollectionComponent
  ],
  entryComponents: [ProductAddComponent, ProductEditComponent]
})
export class ProductModule {}
