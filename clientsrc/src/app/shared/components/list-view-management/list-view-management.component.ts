import { Component, Input, EventEmitter, Output } from '@angular/core';
import { Button } from 'src/app/shared/base-model/button.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionType } from 'src/app/shared/constant/action-type.constant';
import { UserDefinedColumnSetting } from '../../base-model/user-defined-column-setting.model';
import { ComponentBase } from '../component-base';
import { NotificationService } from '../../services/notification.service';
import { Store, select } from '@ngrx/store';
import { FormState } from '../../base-model/form.state';
import { Action } from '../../constant/form-action.constant';
import * as listViewManagementSelector from './state/index';
import * as listViewManagementActions from './state/list-view-management.actions';

@Component({
  selector: 'app-list-view-management',
  templateUrl: './list-view-management.component.html',
  styleUrls: ['./list-view-management.component.css']
})
export class ListViewManagementComponent extends ComponentBase {
  constructor(
    private _store: Store<any>,
    private modalService: NgbModal,
    private _notificationService: NotificationService) {
    super();
  }
  @Input() totalItems = 0;
  public pageNumber = 0;
  @Input() title;
  @Input() datasource;
  @Input() pageSize = 10;
  @Input() listButton;
  @Input() actionType;
  @Input() addSuccessMessage: string;
  @Input() updateSuccessMessage: string;
  @Input() deleteSuccessMessage: string;
  @Input() userDefinedColumnSetting: UserDefinedColumnSetting;
  @Input() isHiddenSearchBox;
  @Output() clickButton = new EventEmitter<any>();
  @Output() changeSelectedPage = new EventEmitter<any>();
  @Output() changeSelectedColumnEvent = new EventEmitter<any>();
  @Output() resultDialog = new EventEmitter<any>();
  @Output() selectedRow = new EventEmitter<any>();
  @Output() searchQuery = new EventEmitter<any>();

  onInit() {
    this.handleSubscription(
      this._store
        .pipe(select(listViewManagementSelector.getFormState))
        .subscribe((formState: FormState) => {
          if (!formState) {
            return;
          }
          if (formState.action === Action.None) {
            return;
          }
          if (formState.error === null) {
            switch (formState.action) {
              case Action.Add:
                this._notificationService.success(this.addSuccessMessage);
                this.closeDialog();
                break;
              case Action.Update:
                this._notificationService.success(this.updateSuccessMessage);
                this.closeDialog();
                break;
              case Action.Delete:
                this._notificationService.success(this.deleteSuccessMessage);
                this.closeDialog();
                break;
            }
          }
        })
    );

    this.handleSubscription(
      this._store.pipe(select(listViewManagementSelector.getTotalItems)).subscribe(
        totalItems => {
          this.totalItems = totalItems;
          if (this.totalItems === (this.pageNumber) * this.pageSize) {
            this.pageNumber = this.pageNumber - 1;
          }
        }
      ));

    this.handleSubscription(
      this._store.pipe(select(listViewManagementSelector.getSelectedPage)).subscribe(
        selectedPage => {
          this.pageNumber = selectedPage;
        }
      ));
  }

  private closeDialog() {
    this.modalService.dismissAll();
    this._store.dispatch(new listViewManagementActions.ChangeSelectedItemAction(null));
  }

  onDestroy() {
    this._store.dispatch(new listViewManagementActions.UpdateFormStateAction({
      action: Action.None,
      error: null
    }));
    this._store.dispatch(new listViewManagementActions.ResetState());
  }
  eventSelectedPage(page: any) {
    if (page !== undefined && page != null) {
      this.changeSelectedPage.emit(page);
      this._store.dispatch(new listViewManagementActions.ChangeSelectedPageAction(page));
    }
  }

  eventSelectedRow(selectedRow: any) {
    if (selectedRow !== undefined && selectedRow != null) {
      const itemId = selectedRow ? selectedRow[0].id : null;
      this._store.dispatch(new listViewManagementActions.ChangeSelectedItemAction(itemId));
      this.selectedRow.emit(selectedRow);
    }
  }

  onClickBtn(item: Button) {
    if (item !== undefined) {
      if (this.actionType === ActionType.dialog) {
        const dialogRef = this.modalService.open(item.component, item.configDialog);
        this.resultDialog.emit(dialogRef);
      }
    }
  }
  eventSearchQuery(event: any) {
    this.searchQuery.emit(event);
  }
}
