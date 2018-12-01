import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import * as productActions from '../state/product.actions';
import { Observable, of } from 'rxjs';
import { Action, Store } from '@ngrx/store';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { ProductService } from 'src/app/shared/services/product.service';
import { ProductModel, ProductListModel, ProductModelRequest } from '../product';
import { PagedResult } from 'src/app/shared/base-model/paged-result';
import * as listViewManagementActions from 'src/app/shared/components/list-view-management/state/list-view-management.actions';
import { FieldTemplateModel } from '../../field-templates/field-template.model';
import { FieldTemplateService } from 'src/app/shared/services/field-template.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { CategoryModel } from '../../categories/category.model';

@Injectable()
export class ProductEffects {
  constructor(
    private store: Store<any>,
    private action$: Actions,
    private productService: ProductService,
    private fieldTemplateService: FieldTemplateService,
    private categoryService: CategoryService
  ) {}

  @Effect()
  getProducts$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.GetProducts),
    mergeMap((action: productActions.GetProducts) =>
      this.productService
        .getAll(action.payload.page, action.payload.numberItemsPerPage, action.queryText)
        .pipe(
          map((products: PagedResult<ProductListModel>) => {
            this.store.dispatch(
              new listViewManagementActions.GetPageSuccessAction(products)
            );
            return new productActions.GetProductsSuccess(products);
          })
        )
    )
  );

  @Effect()
  getFieldTemplates$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.GetFieldTemplates),
    mergeMap((action: productActions.GetFieldTemplates) =>
      this.fieldTemplateService
        .getAllFieldTemplates()
        .pipe(
          map(
            (fieldTemplates: FieldTemplateModel[]) =>
              new productActions.GetFieldTemplatesSuccess(fieldTemplates)
          )
        )
    )
  );

  @Effect()
  getCategories$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.GetCategories),
    mergeMap((action: productActions.GetFieldTemplates) =>
      this.categoryService
        .getAllCategory()
        .pipe(
          map(
            (categories: CategoryModel[]) =>
              new productActions.GetCategoriesSuccess(categories)
          )
        )
    )
  );

  @Effect()
  loadFromTemplate$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.LoadFromTemplate),
    mergeMap((action: productActions.LoadFromTemplate) =>
      this.productService
        .createProductFromTemplate(action.payload)
        .pipe(
          map(
            (productModel: ProductModel) =>
              new productActions.LoadFromTemplateSuccess(productModel)
          )
        )
    )
  );

  @Effect()
  loadFieldTemplateModeEdit$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.LoadFieldTemplateModeEdit),
    mergeMap((action: productActions.LoadFieldTemplateModeEdit) =>
      this.productService
        .createProductFromTemplate(action.payload)
        .pipe(
          map(
            (productModel: ProductModel) =>
              new productActions.LoadFieldTemplateModeEditSuccess(productModel)
          )
        )
    )
  );

  @Effect()
  getProduct$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.GetProduct),
    mergeMap((action: productActions.GetProduct) =>
      this.productService
        .getById(action.payload)
        .pipe(
          map(
            (productModel: ProductModel) =>
              new productActions.GetProductSuccess(productModel)
          )
        )
    )
  );

  @Effect()
  addProduct$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.AddProduct),
    map((action: productActions.AddProduct) => action.payload),
    mergeMap((product: ProductModelRequest) =>
      this.productService.add(product).pipe(
        map(newProduct => {
          this.store.dispatch(new listViewManagementActions.AddSucessAction());
          return new productActions.AddProductSuccess(newProduct);
        }),
        catchError(error => of(new productActions.AddProductFail(error)))
      )
    )
  );
  @Effect()
  updateProduct$: Observable<Action> = this.action$.pipe(
    ofType(productActions.ProductActionTypes.UpdateProduct),
    map((action: productActions.UpdateProduct) => action.payload),
    mergeMap((product: ProductModelRequest) =>
      this.productService.update(product).pipe(
        map(updateProduct => {
          this.store.dispatch(new listViewManagementActions.UpdateSucessAction());
          return new productActions.UpdateProductSuccess({
            id: product.id,
            name: product.name,
            description: product.description
          });
        }),
        catchError(error => of(new productActions.UpdateProductFail(error)))
      )
    )
  );
}
