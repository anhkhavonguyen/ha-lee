import {Component, OnInit, ViewEncapsulation, ViewChild, ElementRef} from '@angular/core';
import {StaffService} from './staff.service';
import {StaffModel, GetStaffRequest} from './staff.model';
import {Observable, fromEvent} from 'rxjs';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {SetPasswordStaffComponent} from 'src/app/containers/staff/change-password-staff/set-password-staff.component';


@Component({
  selector: 'app-staff',
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class StaffComponent implements OnInit {

  public staffs: StaffModel[] = [];
  public staff = new StaffModel();
  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public searchString = '';
  public userRoles!: string[];
  @ViewChild('searchInput')
  searchInput!: ElementRef;
  public loadingIndicator = true;

  constructor(
    private staffService: StaffService,
    private modalService: NgbModal) {
    this.pageNumber = 1;
    this.pageSize = 10;
    this.totalItem = 0;
    this.getUserRole();
  }

  ngOnInit() {
    this.onSearch();
    this.addKeyUpEventToSearchText();
  }

  getUserRole() {
    this.staffService.getUserRole().subscribe(res => {
      this.userRoles = res.roles;
    });
  }

  checkPermissionSetPassword(value: any) {
    if (!this.userRoles) {
      return false;
    }
    const isStaffAdmin = this.userRoles.indexOf('AdminStaff') !== -1;
    if (isStaffAdmin) {
      return false;
    }
    if (value === 'StaffAdminAccount') {
      return false;
    }
    return true;
  }

  setPage(pageInfo: { offset: number; }) {
    const request: GetStaffRequest = {
      pageNumber: pageInfo.offset,
      pageSize: 10,
      searchString: this.searchString,
    };

    this.staffService.getStaffs(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;

      this.staffs = temp.staffModels.map(result => {
        const staffModel = StaffModel.buildStaff(result);
        this.loadingIndicator = false;
        return staffModel;
      });
    });
  }

  onSearch() {
    this.setPage({offset: 0});
  }

  addKeyUpEventToSearchText() {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .subscribe(() => {
        this.onSearch();
      });
  }

  onClickSetPassword(email: string) {
    if (email !== '') {
      const dialogRef = this.modalService.open(SetPasswordStaffComponent, {size: 'lg', centered: true, backdrop: 'static'});
      const instance = dialogRef.componentInstance;
      instance.email = email;
    }
  }
}
