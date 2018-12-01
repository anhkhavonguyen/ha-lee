import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ViewChild } from '@angular/core';
import { OutletModel, GetOutletsRequest } from './outlet.model';
import { OutletService } from './outlet.service';
import { StaffService } from '../staff/staff.service';
import { GetStaffsByOutletRequest, StaffModel } from '../staff/staff.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditOutletComponent } from 'src/app/containers/outlet/edit-outlet/edit-outlet.component';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-outlet',
  templateUrl: './outlet.component.html',
  styleUrls: ['./outlet.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class OutletComponent implements OnInit {

  @ViewChild('myTable') table: any;

  public outlets: OutletModel[] = [];
  public staffs: StaffModel[] = [];
  public pageNumberOutlet: number;
  public pageSizeOutlet: number;
  public totalItemOutlet: number;
  public editedOutlet = new OutletModel();

  public pageNumberStaff: number;
  public pageSizeStaff: number;
  public totalItemStaff: number;

  expanded: any = {};
  timeout: any;
  rows: OutletModel[] = [];
  loadingIndicator = true;
  reorderable = true;

  constructor(
    private outletService: OutletService,
    private staffService: StaffService,
    private modalService: NgbModal,
    private toast: ToastrService,
    private translate: TranslateService) {
    this.pageNumberOutlet = 1;
    this.pageSizeOutlet = 10;
    this.totalItemOutlet = 0;

    this.pageNumberStaff = 1;
    this.pageSizeStaff = 10;
    this.totalItemStaff = 0;
  }
  ngOnInit() {
    this.setPage({ offset: 0 });
  }

  onPage() {
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
    }, 100);
  }

  toggleExpandRow(row: any) {
    this.table.rowDetail.collapseAllRows();
    this.setStaffsPage({ offset: 0 }, row);
  }

  onDetailToggle() {
  }

  setPage(pageInfo: { offset: number; }) {
    const request: GetOutletsRequest = {
      pageNumber: pageInfo.offset,
      pageSize: 10
    };

    this.outletService.getOutlets(request).subscribe(res => {
      const temp = res;
      this.pageNumberOutlet = temp.pageNumber;
      this.pageSizeOutlet = temp.pageSize;
      this.totalItemOutlet = temp.totalItem;

      this.rows = temp.outletModels.map(result => {
        const outletModel = OutletModel.buildOutlet(result);
        this.loadingIndicator = false;
        return outletModel;
      });
      this.sortListOutlet();
    });
  }

  setStaffsPage(pageInfo: { offset: number; }, row: any) {
    const request: GetStaffsByOutletRequest = {
      pageNumber: pageInfo.offset,
      pageSize: 10,
      outletId: row.id
    };

    this.staffService.getStaffsByOutlet(request).subscribe(res => {
      const temp = res;
      this.pageNumberStaff = temp.pageNumber;
      this.pageSizeStaff = temp.pageSize;
      this.totalItemStaff = temp.totalItem;

      const data = temp.staffModels.map(result => {
        const staffModel = StaffModel.buildStaff(result);
        this.loadingIndicator = false;
        return staffModel;
      });

      this.staffs = Object.assign(data);
    });
    this.table.rowDetail.toggleExpandRow(row);
  }

  onClickEditOutlet(outlet: any) {
    const dialogRef = this.modalService.open(EditOutletComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    this.editedOutlet = new OutletModel(outlet);
    instance.outlet = this.editedOutlet;
    instance.listOutlet = this.rows;
    instance.clickSubmitEvent.subscribe((result: any) => {
      if (result) {
        this.updateOutlet();
      }
    });
  }

  checkCountryCode(countryCode: string) {
    return countryCode !== '' ? true : false;
  }

  updateOutlet() {
    this.outletService.updateOutlets(this.editedOutlet).subscribe(res => {
      if (res == null) {
        this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(result => {
          this.toast.error(result);
        });
      } else {
        this.translate.get('APP.EDIT_OUTLET_COMPONENT.UPDATE_SUCCESS').subscribe(result => {
          this.toast.success(result);
          this.setPage({ offset: 0 });
        });
      }
    }, err => {
      this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(result => {
        this.toast.error(result);
      });
    });
  }

  sortListOutlet() {
    if (this.rows !== undefined && this.rows.length > 0) {
      this.rows.sort((a, b) => {
        if (a.code < b.code) {
          return -1;
        } else if (a.code > b.code) {
          return 1;
        } else {
          return 0;
        }
      });
    }
  }
}
