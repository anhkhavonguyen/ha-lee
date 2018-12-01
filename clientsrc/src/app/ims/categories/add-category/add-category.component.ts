import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import * as fromCategory from '../state/category.reducer';
import * as categoryActions from '../state/category.action';
import { CategoryModel } from '../category.model';
import { Guid } from 'src/app/shared/utils/guid.util';
import * as categorySelector from '../state/index';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Action } from 'src/app/shared/constant/form-action.constant';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { FormState } from 'src/app/shared/base-model/form.state';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent extends ComponentBase {
  addValueForm: FormGroup = new FormGroup({});
  constructor(private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store<fromCategory.CategoryState>,
    private notificationService: NotificationService) {
    super();
  }

  onInit() {
    this.addValueForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  onDestroy() { }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onSave() {
    const name = this.addValueForm.get('name').value;
    const description = this.addValueForm.get('description').value;
    const category: CategoryModel = {
      id: Guid.empty(),
      name: name,
      description: description
    };
    this.store.dispatch(new categoryActions.AddCategory(category));
  }
}
