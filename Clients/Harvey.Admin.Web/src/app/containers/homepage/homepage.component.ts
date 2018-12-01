import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import {
  PointTransaction,
  WalletTransaction,
  MembershipTransaction
} from 'src/app/containers/customer/customer-detail/customer-detail.model';
import { Customer } from 'src/app/containers/customer/customer.model';
import {
  FilterCustomerRequest,
  DebitPointTransactionRequest,
  CreditPointTransactionRequest,
  CreditWalletTransactionRequest,
  DebitWalletTransactionRequest,
  VoidOfCreditPointTransactionRequest,
  VoidOfDebitPointTransactionRequest,
  VoidOfDebitWalletTransactionRequest,
  VoidOfCreditWalletTransactionRequest,
  VoidMembershipTransactionRequest
} from 'src/app/containers/homepage/homepage.model';
import { HomepageService } from 'src/app/containers/homepage/homepage.service';
import { OutletModel, GetOutletsRequest } from 'src/app/containers/outlet/outlet.model';
import { OutletService } from 'src/app/containers/outlet/outlet.service';
import { CustomerService } from 'src/app/containers/customer/customer.service';
import { NgbModal, NgbTabset } from '@ng-bootstrap/ng-bootstrap';
import { InfoDialogComponent } from 'src/app/shared/components/info-dialog/info-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { Subscription, Subject } from 'rxjs';
import { AppSettingsService } from '../../shared/services/app-settings.service';
import { AppSettingLabelConstants } from 'src/app/shared/constants/common.constant';
import { MemberSummaryComponent } from 'src/app/containers/homepage/member-summary/member-summary.component';
import { NgbTab } from '@ng-bootstrap/ng-bootstrap/tabset/tabset';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomepageComponent implements OnInit, OnDestroy {

  constructor(
    private homepageService: HomepageService,
    private outletService: OutletService,
    private cutomerService: CustomerService,
    private modalService: NgbModal,
    private toast: ToastrService,
    private appSettingsService: AppSettingsService,
    private ref: ChangeDetectorRef) {
    this.beforeTab = this.activeId = this.statisticsTab;
    this.changeFilterFromDate(this.filteredFromDate);
    this.appSummaryLabelTitle = this.getSummaryLabelTitle(this.appSettingsService.appSettingsDataByAdminType);
    this.appSettingsDataSubcription = this.appSettingsService.appSettingsDataByAdminTypeSubject.subscribe(data => {
      this.appSummaryLabelTitle = this.getSummaryLabelTitle(data);
    });
  }

  public selectedOutlet = undefined;
  public filterBy = '';
  public filteredFromDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
  public filteredToDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
  public fromDateRequest: any;
  public toDateRequest: any;
  public outletRequest = undefined;

  public dateFilter = new Date().toISOString();


  public pageSize = 10;
  public outlets: OutletModel[] = [];
  public appSettingsDataSubcription = new Subscription();
  public appSummaryLabelTitle = '';
  eventUpdateStatisticsData: Subject<any> = new Subject();
  eventUpdateMemberData: Subject<any> = new Subject();
  eventUpdatePointData: Subject<any> = new Subject();
  eventUpdateWalletData: Subject<any> = new Subject();
  statisticsTab = '0';
  memberTab = '1';
  pointTab = '2';
  walletTab = '3';
  activeId: any;

  beforeTab = '';
  hasChangeFromDate = false;
  ngOnInit() {
    this.loadOutlets();
    this.createDateFilter(this.filteredFromDate, this.filteredToDate);
  }

  ngOnDestroy(): void {
    this.appSettingsDataSubcription.unsubscribe();
    this.eventUpdateStatisticsData.unsubscribe();
    this.eventUpdateMemberData.unsubscribe();
    this.eventUpdatePointData.unsubscribe();
    this.eventUpdateWalletData.unsubscribe();
  }

  public loadOutlets() {
    const request: GetOutletsRequest = {
      pageNumber: 0,
      pageSize: 10
    };

    this.outletService.getOutlets(request).subscribe(res => {
      const temp = res;

      this.outlets = temp.outletModels.map(result => {
        const outletModel = OutletModel.buildOutlet(result);
        return outletModel;
      });
    });
  }

  onClickReset() {
    this.hasChangeFromDate = false;
    const now = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    this.changeFilterFromDate(now);
    this.filteredToDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    this.createDateFilter(this.filteredFromDate, this.filteredToDate);
    this.selectedOutlet = undefined;
    this.outletRequest = undefined;
    this.ref.detectChanges();
    this.loadData();
  }

  onClickFilter() {
    this.hasChangeFromDate = true;
    this.createDateFilter(this.filteredFromDate, this.filteredToDate);
    this.outletRequest = this.selectedOutlet;
    this.ref.detectChanges();
    if (this.fromDateRequest > this.toDateRequest) {
      this.toast.warning('The from date cannot be greater than the to date!');
    } else {
      this.loadData();
    }
  }

  loadData() {
    switch (this.activeId) {
      case this.statisticsTab: {
        this.eventUpdateStatisticsData.next();
        break;
      }
      case this.memberTab: {
        this.eventUpdateMemberData.next();
        break;
      }
      case this.pointTab: {
        this.eventUpdatePointData.next();
        break;
      }
      case this.walletTab: {
        this.eventUpdateWalletData.next();
        break;
      }
      default: {
        break;
      }
    }
  }

  createDateFilter(filterFromDate: any, filterToDate: any) {
    if (!filterFromDate) {
      filterFromDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    }
    if (!filterToDate) {
      filterToDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    }
    if (this.activeId === this.statisticsTab) {
      this.fromDateRequest = new Date(filterFromDate.year, filterFromDate.month - 1, filterFromDate.day, 23, 59, 59).toISOString();
    } else {
      this.fromDateRequest = new Date(filterFromDate.year, filterFromDate.month - 1, filterFromDate.day, 0, 0, 0).toISOString();
    }
    this.toDateRequest = new Date(filterToDate.year, filterToDate.month - 1, filterToDate.day, 23, 59, 59).toISOString();
  }

  private getSummaryLabelTitle(appSettingsData: any) {
    if (appSettingsData && Array.isArray(appSettingsData)) {
      const appSummaryLabelTitle = appSettingsData.find((x: any) => x.name === AppSettingLabelConstants.AdminAppSummaryLabelTitleText);
      if (appSummaryLabelTitle && appSummaryLabelTitle.value) {
        return appSummaryLabelTitle.value;
      }
    }

    return '';
  }

  onTabChange(e: any) {
    if (e.nextId) {
      this.activeId = e.nextId;
      this.changeFilterFromDate(this.filteredFromDate);
      this.beforeTab = this.activeId;
      this.createDateFilter(this.filteredFromDate, this.filteredToDate);
    }
  }

  changeFilterFromDate(filterFromDate: any) {
    if (this.activeId === this.statisticsTab && !this.hasChangeFromDate) {
      this.filteredFromDate = {
        year: filterFromDate.year, month: filterFromDate.month,
        day: filterFromDate.day - 1
      };
    } else {
      if (this.beforeTab === this.statisticsTab && !this.hasChangeFromDate) {
        this.filteredFromDate = {
          year: filterFromDate.year, month: filterFromDate.month,
          day: filterFromDate.day + 1
        };
      } else {
        this.filteredFromDate = {
          year: filterFromDate.year, month: filterFromDate.month,
          day: filterFromDate.day
        };
      }
    }
  }
}
