import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromLocation from '../state/location.reducer';
import * as locationActions from '../state/location.action';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { Guid } from 'src/app/shared/utils/guid.util';
import { LocationModel } from '../location.model';

@Component({
  selector: 'app-add-location',
  templateUrl: './add-location.component.html',
  styleUrls: ['./add-location.component.scss']
})
export class AddLocationComponent extends ComponentBase {
  addValueForm: FormGroup = new FormGroup({});
  constructor(
    private activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private store: Store<fromLocation.LocationState>
  ) {
    super();
  }

  onInit() {
    this.addValueForm = this.formBuilder.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      type: [1, Validators.required]
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
    const address = this.addValueForm.get('address').value;
    const type = this.addValueForm.get('type').value;
    const location: LocationModel = {
      id: Guid.empty(),
      name: name,
      address: address,
      type: this.addValueForm.get('type').value
    };
    this.store.dispatch(new locationActions.AddLocation(location));
  }
}
