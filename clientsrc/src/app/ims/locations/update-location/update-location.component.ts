import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as fromLocation from '../state/location.reducer';
import * as locationActions from '../state/location.action';
import * as locationSelector from '../state/index';
import { Store, select } from '@ngrx/store';
import { LocationModel } from '../location.model';
import { takeWhile } from 'rxjs/operators';
import { ComponentBase } from 'src/app/shared/components/component-base';

@Component({
  selector: 'app-update-location',
  templateUrl: './update-location.component.html',
  styleUrls: ['./update-location.component.scss']
})
export class UpdateLocationComponent extends ComponentBase {
  public updateValueForm: FormGroup = new FormGroup({});
  public location: LocationModel;
  public componentActive = true;

  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store<fromLocation.LocationState>,
  ) {
    super();
  }

  onInit() {
    this.updateValueForm = this.formBuilder.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      type: ['']
    });

    this.handleSubscription(this.store.pipe(
      select(locationSelector.getSelectedItem), takeWhile(() => this.componentActive))
      .subscribe(
        (id: string) => {
          if (id == null) {
            return;
          }
          this.store.dispatch(new locationActions.GetLocation(id));
        }
      ));

    this.handleSubscription(this.store.pipe(
      select(locationSelector.getLocation), takeWhile(() => this.componentActive))
      .subscribe(
        (location: LocationModel) => {
          if (location == null) {
            return;
          }
          this.location = location;
          this.updateValueForm.patchValue({
            name: this.location.name,
            address: this.location.address,
            type: this.location.type
          });
        }));
  }

  onDestroy() { }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  onSave() {
    const name = this.updateValueForm.get('name').value;
    const address = this.updateValueForm.get('address').value;
    const location: LocationModel = {
      id: this.location.id,
      name: name,
      address: address,
      type: this.updateValueForm.get('type').value === '1' ? 1 : 2
    };
    this.store.dispatch(new locationActions.UpdateLocation(location));
  }
}
