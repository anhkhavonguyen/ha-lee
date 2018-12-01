import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-qr-code',
  templateUrl: './qr-code.component.html',
  styleUrls: ['./qr-code.component.scss']
})
export class QrCodeComponent implements OnInit {

  @Input() imageCustomer: string;
  @Input() customerCode: string;

  constructor(public activeModal: NgbActiveModal ) { }

  ngOnInit() {
  }

  onClose(): void {
    this.activeModal.close();
  }

}
