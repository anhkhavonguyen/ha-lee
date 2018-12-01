import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import * as fromField from '../state/field.reducer';
import { FieldTypes } from '../field-base/field-type';
import * as fieldActions from '../state/field.action';
import { blankSpaceValidator, validateRequired } from 'src/app/shared/utils/validation.util';
import { FieldModel, EntityRefModel } from '../field.model';
import * as fieldSelector from '../state/index';
import { takeWhile } from 'rxjs/operators';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { Guid } from 'src/app/shared/utils/guid.util';

@Component({
  selector: 'app-add-field',
  templateUrl: './add-field.component.html',
  styleUrls: ['./add-field.component.scss']
})
export class AddFieldComponent extends ComponentBase {

  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store<fromField.FieldState>,
  ) {
    super();
  }

  addValueForm: FormGroup = new FormGroup({});
  fieldTypes;
  selectedType;
  isShowEntityRefList = false;
  isShowPredefinedList = false;
  predefinedList;
  entityRefList = [];
  selectedEntityRef;
  defaultValue = '';
  componentActive = true;

  onInit() {
    this.store.dispatch(new fieldActions.GetEntityRefList());
    this.handleSubscription(
      this.store.pipe(select(fieldSelector.getEntityRefList),
        takeWhile(() => this.componentActive))
        .subscribe(
          (entityRefList: Array<EntityRefModel>) => {
            this.entityRefList = entityRefList;
          }
        ));

    this.addValueForm = this.formBuilder.group({
      name: ['', [Validators.required, blankSpaceValidator]],
      description: [''],
      type: ['', Validators.required],
      defaultValue: [''],
      entityList: [''],
      predefinedList: ['', validateRequired(this.isShowPredefinedList)]
    });

    this.setListFieldTypes();
  }

  onDestroy() {

  }


  setListFieldTypes() {
    this.fieldTypes = Object.keys(FieldTypes).map((key, value) => {
      return { key: key, value: value + 1 };
    });
    this.selectedType = this.fieldTypes[0].value;
  }

  onChangeFieldType(e: any) {
    if (this.selectedType === FieldTypes['Predefined List']) {
      this.isShowPredefinedList = true;
      this.isShowEntityRefList = false;
    } else if (this.selectedType === FieldTypes['Entity Reference']) {
      if (this.entityRefList && this.entityRefList.length > 0) {
        this.selectedEntityRef = this.entityRefList[0].id;
      }
      this.isShowEntityRefList = true;
      this.isShowPredefinedList = false;
    } else {
      this.isShowPredefinedList = false;
      this.isShowEntityRefList = false;
    }
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onSave() {
    const name = this.addValueForm.get('name').value;
    const description = this.addValueForm.get('description').value;
    const field: FieldModel = {
      id: Guid.empty(),
      name: name,
      description: description,
      type: this.selectedType,
      defaultValue: this.setDefaultValue()
    };
    this.store.dispatch(new fieldActions.AddField(field));
  }

  setDefaultValue() {
    if (this.selectedType === FieldTypes['Entity Reference']) {
      const selectedValue = this.entityRefList.find(x => x.id === this.selectedEntityRef);
      if (selectedValue) {
        this.defaultValue = '';
        Object.keys(selectedValue).map((key, value) => {
          this.defaultValue += `"${key}": "${selectedValue[key]}",`;
        });
        this.defaultValue = this.defaultValue.substring(0, this.defaultValue.length - 1);
        this.defaultValue = `{ ${this.defaultValue} }`;
      }
    } else if (this.selectedType === FieldTypes['Predefined List'] && this.predefinedList !== undefined) {
      this.defaultValue = '';
      this.predefinedList.forEach(element => {
        this.defaultValue += `${element.value},`;
      });
      this.defaultValue = this.defaultValue.substring(0, this.defaultValue.length - 1);
    } else {
      this.defaultValue = undefined;
    }
    return this.defaultValue;
  }

}
