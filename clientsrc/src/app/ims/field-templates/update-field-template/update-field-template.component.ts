import { Component, OnInit } from '@angular/core';
import {
  Section, FieldTemplateTypes, FieldFieldTemplateActions,
  FieldFieldTemplateModel, FieldTemplateModel, FieldFieldTemplateAction, FieldTemplateType
} from '../field-template.model';
import { FieldModel, EntityRefModel } from 'src/app/ims/fields/field.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import * as fromFieldTemplate from '../state/field-template.reducer';
import * as fieldTemplateActions from '../state/field-template.action';
import * as fieldTemplateSelector from '../state/index';
import { takeWhile } from 'rxjs/operators';
import { FieldTemplateService } from 'src/app/shared/services/field-template.service';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { FieldType, FieldTypes } from '../../fields/field-base/field-type';
import { checkJsonString } from 'src/app/shared/utils/JSON.util';

const textSectionName = 'Section';

@Component({
  selector: 'app-update-field-template',
  templateUrl: './update-field-template.component.html',
  styleUrls: ['./update-field-template.component.css']
})
export class UpdateFieldTemplateComponent extends ComponentBase {

  constructor(private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store<fromFieldTemplate.FieldTemplateState>,
    private fieldTemplateService: FieldTemplateService) {
      super();
     }

  section: Section[] = [];
  isVariant: boolean;
  defaultSection: Section = new Section({
    name: 'Variant',
    isVariantField: true,
    orderSection: -1,
    fields: Array<FieldModel>()
  });
  componentActive = true;
  listField: FieldModel[] = [];
  isSelected = true;
  selectedSection: Section;
  fieldTemplateTypes: Array<string> = [];
  FieldTemplateTypes = FieldTemplateTypes;
  FieldFieldTemplateActions = FieldFieldTemplateActions;
  updateValueForm: FormGroup = new FormGroup({});
  public itemId = null;
  public fieldTemplate: FieldTemplateModel;
  public fieldTemplateTemp: FieldTemplateModel;
  type: boolean;
  isVariantField = true;
  supportWithVariant = false;
  orderSection = 0;
  selectedField: any;

  onInit() {
    this.getFieldTemplateTypes();
    this.getSelectedItem();

    this.updateValueForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      type: [''],
      field: [''],
      sectionName: [''],
      listField: ['']
    });

    this.handleSubscription(this.fieldTemplateService.getBy(this.itemId).subscribe(result => {
      this.fieldTemplate = new FieldTemplateModel(result);
      this.fieldTemplateTemp = new FieldTemplateModel(result);
      if (result && result.sections) {
        this.section = new Array<Section>();
        result.sections.forEach(element => {
          const newSection = new Section(element);
          this.section.push(newSection);
        });
        this.getFields(result);
        this.type = this.fieldTemplate.type == FieldTemplateType.WithVariant.toString() ? true : false;
      }
      this.updateValueForm.patchValue({
        name: this.fieldTemplate.name,
        description: this.fieldTemplate.description
      });
    }));
  }

  filterListField(element: Section) {
    element.fields.forEach(field => {
      if (this.listField && this.listField.length > 0) {
        this.listField = this.listField.filter(x => {
          return x.id !== field.field.id;
        });
      }
    });
  }

  onDestroy() {
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onChangeType(e: any) {
    if (e.target.checked) {
      this.section.unshift(this.defaultSection);
    } else {
      if (this.section) {
        this.section = this.section.filter((item) => {
          return item.orderSection !== -1;
        });
      }
    }
  }

  getFields(fieldTemplate: FieldTemplateModel) {
    this.store.dispatch(new fieldTemplateActions.GetFields());
    this.store.pipe(select(fieldTemplateSelector.getFields),
      takeWhile(() => this.componentActive))
      .subscribe(
        (fields: any) => {
          this.listField = fields;
          fieldTemplate.sections.forEach(element => {
            this.filterListField(element);
          });
        }
      );
  }

  getFieldTemplateTypes() {
    this.store.pipe(select(fieldTemplateSelector.getFieldTemplateTypes),
      takeWhile(() => this.componentActive))
      .subscribe(
        (fieldTemplateTypes: Array<string>) => {
          this.fieldTemplateTypes = fieldTemplateTypes;
        }
      );
  }

  onClickAddSection(e: any) {
    this.orderSection = Math.max.apply(Math, this.section.map(function(o) { return o.orderSection; })) + 1;
    const count = this.section.length;
    const tempSection = new Section({
      name: `${textSectionName}_${count + 1}`,
      isVariantField: false,
      orderSection: this.orderSection,
      fields: Array<FieldFieldTemplateModel>()
    });
    this.section = Array.from(this.section);
    this.section.push(tempSection);
    this.fieldTemplateTemp.sections.push(new Section(tempSection));
  }

  onClickAddField(orderSection: number) {
    this.isSelected = false;
    this.selectedSection = this.section.filter((value) => {
      return value.orderSection === orderSection;
    }).pop();
    this.selectedField = [''];
  }

  onSelectdField(e: FieldModel) {
    if (e && this.selectedSection) {
      const newFieldFieldTemplate = new FieldFieldTemplateModel({
        field: e
      });
      this.selectedSection.fields = Array.from(this.selectedSection.fields);
      this.selectedSection.fields.push(newFieldFieldTemplate);
      this.fieldTemplateTemp.sections.forEach(element => {
        if (element.orderSection === this.selectedSection.orderSection) {
          const newField = new FieldFieldTemplateModel({
            field: e,
            action: FieldFieldTemplateAction.Add
          });
          if (element.fields.find(x => x.field.id === e.id)) {
            element.fields.forEach(x => {
              if (x.field.id === e.id) {
                x.action = FieldFieldTemplateAction.Add.toString();
              }
            });
          } else {
            element.fields.push(newField);
          }
        }
      });
    }
    this.isSelected = true;
    this.selectedSection = null;
    this.resetListField(e);
    this.isVariantField = this.checkVariantField();
  }

  resetListField(e: FieldModel) {
    this.listField = this.listField.filter((item) => {
      return item.id !== e.id;
    });
  }


  onDeleteSection(e: Section) {
    this.fieldTemplateTemp.sections.forEach((section) => {
      if (section.name == e.name) {
        section.fields.forEach((fieldFieldTemplate) => {
          fieldFieldTemplate.action = FieldFieldTemplateAction.Delete.toString();
        });
      }
    });
    if (this.section && this.section.length > 0) {
      this.section = this.section.filter((item) => {
        return item !== e;
      });
      e.fields.forEach((item) => {
        this.listField.push(item.field);
      });
      this.listField = Array.from(this.listField);
    }
  }

  onDeleteField(section: Section, field: FieldModel) {
    if (field && section) {
      this.fieldTemplateTemp.sections.forEach((item) => {
        if (item.name === section.name) {
          item.fields.forEach((fieldFieldTemplate) => {
            if (fieldFieldTemplate.field.id == field.id) {
              fieldFieldTemplate.action = FieldFieldTemplateAction.Delete.toString();
            }
          });
        }
      });
      section.fields = section.fields.filter((item) => {
        return item.field.id !== field.id;
      });
      this.listField.push(field);
      this.listField = Array.from(this.listField);
    }
    this.isVariantField = this.checkVariantField();
    this.selectedField = [''];
  }

  onSave() {
    const name = this.updateValueForm.get('name').value;
    const description = this.updateValueForm.get('description').value;
    const fieldTemplate: FieldTemplateModel = {
      id: this.fieldTemplate ? this.fieldTemplate.id : null,
      name: name,
      description: description,
      sections: this.fieldTemplateTemp.sections,
      type: this.updateValueForm.get('type') && this.updateValueForm.get('type').value
        ? FieldTemplateType.WithVariant.toString()
        : FieldTemplateType.WithoutVariant.toString()
    };
    this.store.dispatch(new fieldTemplateActions.UpdateFieldTemplate(fieldTemplate));
    this.activeModal.close('closed');
  }

  getSelectedItem() {
    this.handleSubscription(this.store.pipe(select(fieldTemplateSelector.getSelectedItem),
      takeWhile(() => this.componentActive))
      .subscribe(
        (id: string) => {
          this.itemId = id;
        }
      ));
  }

  getFieldType(type: any) {
    return Object.values(FieldTypes).includes(+type)
    ? Object.keys(FieldTypes).find(function(item, key) { return key === (+type - 1); })
    : type;
  }

  buildDefaultValue(value: any, type: string) {
    if (type.toString() === FieldType.EntityReference.toString()) {
        if (checkJsonString(value)) {
            const object = new EntityRefModel(JSON.parse(value));
            return object.name;
        }
    }
    return value;
}

  checkVariantField () {
    const existVariant = this.section.find(x => x.isVariantField === true);
    if (existVariant.fields.length > 0) {
      return true;
    }
    return false;
  }
}
