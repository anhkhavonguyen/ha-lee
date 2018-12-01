import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store, select } from '@ngrx/store';
import * as fromCategory from '../state/category.reducer';
import * as categoryActions from '../state/category.action';
import * as categorySelector from '../state/index';
import { takeWhile } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Action } from 'src/app/shared/constant/form-action.constant';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { FormState } from 'src/app/shared/base-model/form.state';
@Component({
  selector: 'app-delete-category',
  templateUrl: './delete-category.component.html',
  styleUrls: ['./delete-category.component.css']
})
export class DeleteCategoryComponent extends ComponentBase {
  public itemId: string;
  public componentActive = true;

  constructor(private activeModal: NgbActiveModal,
    private store: Store<fromCategory.CategoryState>) {
    super();
  }

  onInit() {
    this.handleSubscription(
      this.store
        .pipe(select(categorySelector.getSelectedItem), takeWhile (() => this.componentActive))
        .subscribe (
          (id: string) => {
            if (id == null) {
              return;
            }
            this.itemId = id;
          }
      ));
  }

  onDestroy() { }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onSave() {
    this.store.dispatch(new categoryActions.DeleteCategory(this.itemId));
  }
}
