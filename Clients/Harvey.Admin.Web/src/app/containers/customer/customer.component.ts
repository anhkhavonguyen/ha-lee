import { Component, OnInit } from '@angular/core';
import { CustomerService } from 'src/app/containers/customer/customer.service';
import { CustomersRequest, Customer } from 'src/app/containers/customer/customer.model';
import { ViewEncapsulation } from '@angular/core';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { fromEvent } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadFileComponent } from 'src/app/shared/components/upload-file/upload-file.component';
import { ExportCSVService } from 'src/app/shared/services/export-csv.service';
import { ToastrService } from 'ngx-toastr';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerComponent implements OnInit {

  constructor(
    private cutomerService: CustomerService,
    private modalService: NgbModal,
    private exportCSVService: ExportCSVService,
    private toast: ToastrService,
    private appSettingService: AppSettingsService,
    private translate: TranslateService) {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
  }

  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public customerList: Array<Customer> = [];
  public loadingIndicator = true;
  public searchText = '';
  public isLoading = false;
  @ViewChild('searchInput')
  searchInput!: ElementRef;

  ngOnInit() {
    window.scrollTo(0, 0);
    this.onSearch();
    this.addKeyUpEventToSearchText();
  }

  public loadCustomers(request: CustomersRequest) {
    this.cutomerService.GetCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;
      this.loadingIndicator = false;
      this.customerList = temp.customerListResponse.map(result => {
        const customerModel = Customer.buildCustomer(result);
        return customerModel;
      });
    });
  }

  setPage(pageInfo: { offset: number; }) {
    const request: CustomersRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      searchText: this.searchText,
      outletId: '',
      dateFilter: ''
    };
    this.loadCustomers(request);
  }

  onSearch() {
    this.setPage({ offset: 0 });
  }

  addKeyUpEventToSearchText() {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .subscribe(() => {
        this.onSearch();
      });
  }

  onClickUploadFile() {
    const dialogRef = this.modalService.open(UploadFileComponent, { size: 'lg', centered: true, backdrop: 'static' });
  }

  onClickExportCSV() {
    this.isLoading = true;
    this.exportCSVService.ExportCustomerCSV().subscribe((data) => {
      const blob = new Blob([data], { type: 'text/csv' });
      const url = window.URL.createObjectURL(blob);

      if (navigator.msSaveOrOpenBlob) {
        navigator.msSaveBlob(blob, 'Customers.csv');
      } else {
        const a = document.createElement('a');
        a.href = url;
        a.download = 'Customers.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
      }
      window.URL.revokeObjectURL(url);
      this.isLoading = false;
    });
  }

  onClickUpdateGender() {
    this.isLoading = true;
    this.cutomerService.UpdateGender(null).subscribe((data) => {
      if (data === 1) {
        this.toast.success('Updated successfully!');
      } else {
        this.toast.success('Updated failed!');
      }
      this.isLoading = false;
    });
  }
}
