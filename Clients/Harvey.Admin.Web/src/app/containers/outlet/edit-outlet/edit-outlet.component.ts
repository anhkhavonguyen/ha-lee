import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Observable, zip } from 'rxjs';
import { OutletModel } from 'src/app/containers/outlet/outlet.model';
import { Input, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { AbstractControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { GeneralValidation, blankSpaceValidator } from 'src/app/shared/utils/validation.util';
import { OutletService } from 'src/app/containers/outlet/outlet.service';
import { TranslateService } from '@ngx-translate/core';
import { Output } from '@angular/core';
import { fail } from 'assert';

@Component({
    selector: 'app-edit-outlet',
    templateUrl: './edit-outlet.component.html',
    styleUrls: ['./edit-outlet.component.scss']
})
export class EditOutletComponent implements OnInit {

    constructor(
        private activeModal: NgbActiveModal,
        private formBuilder: FormBuilder,
        private toast: ToastrService,
        private outletService: OutletService,
        private translate: TranslateService) {
    }
    isLoading = false;
    @Input() outlet = new OutletModel();
    @Input() listOutlet: OutletModel[] = [];
    editOutletForm: FormGroup = new FormGroup({});
    isUpdating = false;
    isIconUpdating = false;
    icon = '';
    displayIcon = '';
    currentOutlet: any;
    @Output() clickSubmitEvent = new EventEmitter<boolean>();

    ngOnInit() {
        this.icon = this.outlet.outletImage;
        this.displayIcon = `data:image/png;base64,${this.icon}`;
        this.editOutletForm = this.formBuilder.group({
            outletName: ['', [Validators.required, blankSpaceValidator]],
            outletAddress: ['', [Validators.required, blankSpaceValidator]],
            phoneGroup: this.formBuilder.group({
                phoneCountryCode: ['', Validators.pattern(GeneralValidation.PHONE_REGEX)],
                phoneNumber: ['', [Validators.required, blankSpaceValidator, Validators.pattern(GeneralValidation.PHONE_REGEX)]]
            })
        });
    }

    onClose(): void {
        this.activeModal.close('closed');
    }

    onDismiss(reason: String): void {
        this.activeModal.dismiss(reason);
    }

    onClickUpdateBtn(e: any) {
        this.isLoading = true;
        this.outlet.outletImage = this.icon;
        this.currentOutlet = this.listOutlet.find(a => a.id ===  this.outlet.id);
        if ( this.currentOutlet.name === this.outlet.name &&
            this.currentOutlet.phone === this.outlet.phone &&
            this.currentOutlet.phoneCountryCode === this.outlet.phoneCountryCode &&
            this.currentOutlet.address === this.outlet.address &&
            this.currentOutlet.outletImage === this.outlet.outletImage) {
            this.translate.get('APP.OUTLET_COMPONENT.NOTHING_CHANGE').subscribe(result => {
                this.toast.warning(result);
            });
        } else {
            this.clickSubmitEvent.emit(true);
            this.onClose();
        }
        this.isLoading = false;
    }

    uploadIcon(fileSelected: any) {
        const imgFile = fileSelected.target.files;
        const file = imgFile[0];
        const fileSize = file.size;
        const fileType = file.type;
        const allowedSize = 2097152;
        const allowedType = 'image';
        if (imgFile && file) {
          if (fileSize <= allowedSize && fileType.includes(allowedType)) {
            const reader = new FileReader();
            reader.onload = this.convertoBase64.bind(this);
            reader.readAsBinaryString(file);
          } else {
            if (fileSize > allowedSize) {
              this.translate.get('APP.ERROR.LIMIT_FILE_SIZE').subscribe(result => {
                this.toast.error(result);
              });
            }
            if (!fileType.includes(allowedType)) {
              this.translate.get('APP.ERROR.ALLOWED_FILE_TYPE').subscribe(result => {
                this.toast.error(result);
              });
            }
          }
        }
      }

      convertoBase64(file: any) {
        const binaryString = file.target.result;
        const convertImg = btoa(binaryString);
        this.isIconUpdating = true;
        this.icon = convertImg;
        this.displayIcon = `data:image/png;base64,${convertImg}`;
      }
}
