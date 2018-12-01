import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '../../../../../node_modules/@ng-bootstrap/ng-bootstrap';
import { CustomerDetailService } from '../../../containers/customer/customer-detail/customer-detail.service';
import { Observable } from '../../../../../node_modules/rxjs';
import { ToastrService } from '../../../../../node_modules/ngx-toastr';
import { TypeConfirm } from 'src/app/containers/customer/customer-detail/customer-detail.model';

@Component({
    selector: 'app-confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

    @Input()
    typeConfirm!: TypeConfirm;
    @Input()
    request: any;
    @Input()
    title: any;
    @Input()
    content: any;

    public isLoading = false;
    public isVoided = false;

    constructor(private customerDetailService: CustomerDetailService, public activeModal: NgbActiveModal, private toast: ToastrService) {

    }

    ngOnInit() {

    }

    onDismiss(reason: String): void {
        this.activeModal.dismiss(reason);
    }

    onConfirm(): void {
        this.isLoading = true;
        this.isVoided = true;
        if (this.typeConfirm === TypeConfirm.VoidPoint) {
            this.voidPoint().subscribe(result => {
                this.isLoading = false;
                if (result < 0) {
                    this.toast.warning('Can\'t perform action!');
                }
                this.activeModal.close(result);
            });
        }

        if (this.typeConfirm === TypeConfirm.VoidWallet) {
            this.voidWallet().subscribe(result => {
                this.isLoading = false;
                if (result < 0) {
                    this.toast.warning('Can\'t perform action!');
                }
                this.activeModal.close(result);
            });
        }

        if (this.typeConfirm === TypeConfirm.VoidMembership) {
            this.voidMembership().subscribe(result => {
                this.isLoading = false;
                if (!result) {
                    this.toast.warning('Can\'t perform action!');
                }
                this.activeModal.close(result);
            });
        }

        if (this.typeConfirm === TypeConfirm.ChangeStatusCustomer) {
            this.changeStatusMember().subscribe(result => {
                this.isLoading = false;
                if (result) {
                    this.toast.success('Change status successfully!');
                }
                this.activeModal.close(result);
            });
        }
    }

    voidPoint(): Observable<any> {
        return this.customerDetailService.voidPointTransaction(this.request);
    }

    voidWallet(): Observable<any> {
        return this.customerDetailService.voidWalletTransaction(this.request);
    }

    voidMembership(): Observable<any> {
        return this.customerDetailService.voidMembershipTransaction(this.request);
    }

    changeStatusMember(): Observable<any> {
        return this.customerDetailService.changeStatusMember(this.request);
    }
}
