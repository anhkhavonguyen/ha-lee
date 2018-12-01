import { Component } from '@angular/core';
import { Store, select } from '@ngrx/store';
import * as fromProduct from '../state';
import * as productActions from '../state/product.actions';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { FieldValueModel, EditProductModel, VariantModel, ProductModelRequest, VariantModelRequest, ProductModel } from '../product';
import { FieldValue } from '../../fields/field-base/field-value';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Guid } from 'src/app/shared/utils/guid.util';
import { FieldType } from '../../fields/field-base/field-type';
import { takeWhile } from 'rxjs/operators';
import { FieldTemplateModel } from '../../field-templates/field-template.model';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { isArray } from 'util';
import { CategoryModel } from '../../categories/category.model';

const emptyGuid = '00000000-0000-0000-0000-000000000000';

@Component({
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent extends ComponentBase {
  public componentActive = true;
  updateValueForm: FormGroup = new FormGroup({});
  product: EditProductModel;
  fieldValues: FieldValue<any>[] = [];
  variants: VariantModel[];
  categories: CategoryModel[];
  selectedCategory: any;
  invalidVariant = [];
  invalidFields = [];
  columnCount = 0;
  isDuplicateVariant = false;
  fieldTemplate: ProductModel;
  constructor(
    private activeModal: NgbActiveModal,
    private store: Store<fromProduct.State>,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService
  ) {
    super();
  }

  onInit() {
    this.updateValueForm = this.formBuilder.group({
      name: [''],
      description: [''],
      templateName: [{ value: '', disabled: true }],
      category: [{ value: '' }],
    });

    this.getFieldValues();
    this.handleSubscription(
      this.store
        .pipe(
          select(fromProduct.getSelectedItem),
          takeWhile(() => this.componentActive)
        )
        .subscribe((id: string) => {
          if (id == null) {
            return;
          }
          this.store.dispatch(new productActions.GetProduct(id));
        })
    );

    this.store.dispatch(new productActions.GetCategories());

    this.handleSubscription(
      this.store
        .pipe(select(fromProduct.getCategories))
        .subscribe(categories => (this.categories = categories))
    );

    this.handleSubscription(
      this.store
        .pipe(select(fromProduct.getProduct), takeWhile(() => this.componentActive))
        .subscribe(editProductModel => {
          this.product = editProductModel;
          if (this.product == null) {
            return;
          }
          if (this.product.fieldTemplateId && this.product.fieldTemplateId !== emptyGuid) {
            this.getFieldTemplate(this.product.fieldTemplateId);
          }
          this.updateValueForm.patchValue({
            name: this.product.name,
            description: this.product.description,
            templateName: this.product.fieldTemplateName,
            category: this.product.categoryId ? this.product.categoryId : null
          });
          if (this.product && this.product.variants && this.product.variants.length > 0) {
            const datasource = this.product.variants.map(item => {
              return {
                fieldValues: item.fieldValues,
                price: item.price,
                name: '',
                orderSection: 0,
                isVariantField: true,
                id: item.id
              };
            });
            this.store.dispatch(new productActions.ChangeFieldValuesRequest(datasource));
          } else {
            this.store.dispatch(new productActions.ChangeFieldValuesRequest([]));
          }
        })
    );

    this.handleSubscription(
      this.store
        .pipe(select(fromProduct.getFieldTemplatesModeEdit))
        .subscribe(productModel => {
          this.fieldTemplate = productModel;
          if (this.fieldTemplate && this.fieldTemplate.variants && this.fieldTemplate.variants.length > 0) {
            this.fieldValues = this.fieldTemplate.variants[0].fieldValues;
            if (this.fieldValues) {
              this.columnCount = this.fieldValues.length + 2;
            }
          } else {
            this.fieldValues = [];
          }
        })
    );
  }

  getFieldTemplate(fieldTemplateId: string) {
    this.store.dispatch(new productActions.LoadFieldTemplateModeEdit(fieldTemplateId));
  }
  onDestroy() { }
  onClose(): void {
    this.activeModal.close('closed');
  }
  onTabChange(e: any) {
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  public onSaveField(field: FieldValue<any>) {
    if (field.type === FieldType.Tags
      || field.type === FieldType.EntityReference) {
      field.value = this.buildFieldValueToSave(field);
    }
    this.store.dispatch(new productActions.SaveField(field));
  }

  onSave() {
    const name = this.updateValueForm.get('name').value;
    const description = this.updateValueForm.get('description').value;
    const fields: FieldValueModel[] = [];
    this.product.sections.map(section => {
      section.fieldValues.map(field => {
        fields.push({
          id: field.id,
          fieldId: field.fieldId,
          fieldValue: this.getFieldValue(field)
        });
      });
    });
    const variants = this.getVariantsRequest();
    if (this.isDuplicateVariant) {
      this.notificationService.error('Duplicate variant!');
    } else {
      if (this.invalidVariant.length > 0 || this.invalidFields.length > 0) {
        this.notificationService.error('Please fill in all fields!');
      } else {
        const productModel: ProductModelRequest = {
          id: this.product.id,
          name: name,
          description: description,
          fieldTemplateId: this.product.fieldTemplateId,
          productFields: fields,
          variants: variants,
          categoryId: this.selectedCategory
        };
        this.store.dispatch(new productActions.UpdateProduct(productModel));
      }
    }
  }

  private getFieldValue(field: FieldValue<any>) {
    this.invalidFields = [];
    switch (field.type) {
      case FieldType.EntityReference:
        return field.value;
      case FieldType.PredefinedList:
        if (field.value === null) {
          this.invalidFields.push(field);
        }
        return field.value === null ? null : field.value.join(',');
      default:
        return field.value;
    }
  }

  private getVariantsRequest() {
    this.invalidVariant = [];
    const arrayValue = [];
    this.isDuplicateVariant = false;
    const variants: VariantModelRequest[] = this.variants.map(item => {
      let temp = '';
      const variantFields: FieldValueModel[] = item.fieldValues.map(fieldValue => {
        const fieldValueModel: FieldValueModel = {
          id: fieldValue.id,
          fieldId: fieldValue.fieldId,
          fieldValue: fieldValue.value === null ? null : fieldValue.value.join(',')
        };
        temp += `,${fieldValueModel.fieldValue}`;
        if (fieldValueModel.fieldValue === null) {
          this.invalidVariant.push(fieldValueModel);
        }
        return fieldValueModel;
      });
      if (arrayValue.includes(temp)) {
        this.isDuplicateVariant = true;
      } else {
        arrayValue.push(temp);
      }
      return {
        id: item.id,
        variantFields: variantFields,
        prices: item.price
      };
    });
    return variants;
  }

  getFieldValues() {
    this.handleSubscription(
      this.store
        .pipe(select(fromProduct.getVariants))
        .subscribe((variants: VariantModel[]) => {
          (this.variants = variants);
        })
    );
  }

  buildValue(field: FieldValue<string>) {
    switch (field.type) {
      case FieldType.Tags:
        if (field.value.includes(',')) {
          return field.value.split(',');
        } else {
          return [field.value];
        }
        break;
      case FieldType.EntityReference:
        if (field && field.value) {
          return {
            id: field.fieldId,
            value: field.value
          };
        }
        break;
      default:
        break;
    }
  }

  buildFieldValueToSave(field: FieldValue<any>) {
    switch (field.type) {
      case FieldType.Tags:
        if (isArray(field.value)) {
          return field.value.join(',');
        } else {
          return '';
        }
        break;
      case FieldType.EntityReference:
        if (field.value) {
          return field.value.value;
        } else {
          return '';
        }
      default:
        break;
    }
  }
}
