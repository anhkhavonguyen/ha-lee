import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Customer } from '../../models/customer.model';
import { GetExpiryPointsResponse, GetExpiryPointsRequest, ExpiryPoint } from '../../models/pointTransaction.model';
import { PointTransactionService } from '../../services/point-transactions.service';
import * as moment from 'moment';

@Component({
  selector: 'app-expiry-point-dialog',
  templateUrl: './expiry-point-dialog.component.html',
  styleUrls: ['./expiry-point-dialog.component.scss']
})
export class ExpiryPointDialogComponent implements OnInit {

  @Input() getExpiryPointsResponse: GetExpiryPointsResponse;

  constructor(private activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }
}
