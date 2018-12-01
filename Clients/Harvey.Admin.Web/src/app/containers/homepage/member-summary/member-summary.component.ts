import { Component, OnInit, Input } from '@angular/core';
import { Customer } from 'src/app/containers/customer/customer.model';
import { CustomerService } from 'src/app/containers/customer/customer.service';
import { HomepageService } from 'src/app/containers/homepage/homepage.service';
import { FilterCustomerRequest, GetCustomersByCustomerCodesRequest } from 'src/app/containers/homepage/homepage.model';
import { Subject } from 'rxjs/internal/Subject';
import { ActivityService } from '../../activity/activity.service';
import { FilterHistoryCustomerActivitiesRequest } from '../../activity/activity.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InfoDialogComponent } from 'src/app/shared/components/info-dialog/info-dialog.component';

@Component({
  selector: 'app-member-summary',
  templateUrl: './member-summary.component.html',
  styleUrls: ['./member-summary.component.scss']
})

export class MemberSummaryComponent implements OnInit {

  public newBasicCustomerList: Array<Customer> = [];
  public totalItemNewBasicCustomers = 0;
  public isCollapseNewBasicCustomers = false;
  public pageNumberNewBasicCustomers = 0;
  public loadingIndicatorNewBasicCustomers = true;
  public isLoadingNewBasicCustomers = false;

  public upgradedCustomerList: Array<Customer> = [];
  public totalItemUpgradedCustomers = 0;
  public isCollapseUpgradedCustomers = false;
  public pageNumberUpgradedCustomers = 0;
  public loadingIndicatorUpgradedCustomers = true;
  public isLoadingUpgradedCustomers = false;

  public renewedCustomerList: Array<Customer> = [];
  public totalItemRenewedCustomers = 0;
  public isCollapseRenewedCustomers = false;
  public pageNumberRenewedCustomers = 0;
  public loadingIndicatorRenewedCustomers = true;
  public isLoadingRenewedCustomers = false;

  public extendedCustomerList: Array<Customer> = [];
  public totalItemExtendedCustomers = 0;
  public isCollapseExtendedCustomers = false;
  public pageNumberExtendedCustomers = 0;
  public loadingIndicatorExtendedCustomers = true;
  public isLoadingExtendedCustomers = false;

  public expiredCustomerList: Array<Customer> = [];
  public totalItemExpiredCustomers = 0;
  public isCollapseExpiredCustomers = false;
  public pageNumberExpiredCustomers = 0;
  public loadingIndicatorExpiredCustomers = false;
  public isLoadingExpiredCustomers = false;

  public downgradedCustomerList: Array<Customer> = [];
  public totalItemDowngradedCustomers = 0;
  public isCollapseDowngradedCustomers = false;
  public pageNumberDowngradedCustomers = 0;
  public loadingIndicatorDowngradedCustomers = true;
  public isLoadingDowngradedCustomers = false;

  public activatedCustomerList: Array<Customer> = [];
  public totalItemActivatedCustomers = 0;
  public isCollapseActivatedCustomers = false;
  public pageNumberActivatedCustomers = 0;
  public loadingIndicatorActivatedCustomers = true;
  public isLoadingActivatedCustomers = false;

  public deactivatedCustomerList: Array<Customer> = [];
  public totalItemDeactivatedCustomers = 0;
  public isCollapseDeactivatedCustomers = false;
  public pageNumberDeactivatedCustomers = 0;
  public loadingIndicatorDeactivatedCustomers = true;
  public isLoadingDeactivatedCustomers = false;

  @Input() outletRequest = undefined;
  @Input() fromDateRequest: any;
  @Input() toDateRequest: any;
  @Input() appSummaryLabelTitle = '';
  public pageSize = 10;
  @Input() eventUpdateMemberData: Subject<any> = new Subject();

  constructor(
    private homepageService: HomepageService,
    private activityService: ActivityService,
    private customerService: CustomerService,
    private modalService: NgbModal
  ) {
  }

  ngOnInit() {
    this.setDataSummary();
    this.eventUpdateMemberData.subscribe(event => {
      this.setDataSummary();
    });
  }

  setIsLoadingData() {
    this.isLoadingNewBasicCustomers = true;
    this.isLoadingDeactivatedCustomers = true;
    this.isLoadingDowngradedCustomers = true;
    this.isLoadingExpiredCustomers = true;
    this.isLoadingExtendedCustomers = true;
    this.isLoadingRenewedCustomers = true;
    this.isLoadingUpgradedCustomers = true;
    this.isLoadingActivatedCustomers = true;
  }

  setDataSummary() {
    this.setIsLoadingData();
    this.setNewBasicMembersPage({
      offset: 0
    });
    this.setUpgradedMembersPage({
      offset: 0
    });
    this.setExtendedMembersPage({
      offset: 0
    });
    this.setRenewedMembersPage({
      offset: 0
    });
    this.setExpiredMembersPage({
      offset: 0
    });
    this.setDowngradedMembersPage({
      offset: 0
    });
    this.setActivatedMembersPage({
      offset: 0
    });
    this.setDeactivatedMembersPage({
      offset: 0
    });
  }
  public loadNewBasicCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorNewBasicCustomers = true;
    this.homepageService.GetNewBasicCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberNewBasicCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemNewBasicCustomers = temp.totalItem;
      this.loadingIndicatorNewBasicCustomers = false;
      this.isLoadingNewBasicCustomers = false;
      if (temp.customerListResponse != null) {
        this.newBasicCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadUpgradedCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorUpgradedCustomers = true;
    this.homepageService.GetUpgradedCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberUpgradedCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemUpgradedCustomers = temp.totalItem;
      this.loadingIndicatorUpgradedCustomers = false;
      this.isLoadingUpgradedCustomers = false;
      if (temp.customerListResponse != null) {
        this.upgradedCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadRenewedCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorRenewedCustomers = true;
    this.homepageService.GetRenewedCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberRenewedCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemRenewedCustomers = temp.totalItem;
      this.loadingIndicatorRenewedCustomers = false;
      this.isLoadingRenewedCustomers = false;
      if (temp.customerListResponse != null) {
        this.renewedCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadExtendedCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorExtendedCustomers = true;
    this.homepageService.GetExtendedCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberExtendedCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemExtendedCustomers = temp.totalItem;
      this.loadingIndicatorExtendedCustomers = false;
      this.isLoadingExtendedCustomers = false;
      if (temp.customerListResponse != null) {
        this.extendedCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadExpiredCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorExpiredCustomers = true;
    this.homepageService.GetExpiredCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberExpiredCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemExpiredCustomers = temp.totalItem;
      this.loadingIndicatorExpiredCustomers = false;
      this.isLoadingExpiredCustomers = false;
      if (temp.customerListResponse != null) {
        this.expiredCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadDowngradedCustomers(request: FilterCustomerRequest) {
    this.loadingIndicatorDowngradedCustomers = true;
    this.homepageService.GetDowngradedCustomers(request).subscribe(res => {
      const temp = res;
      this.pageNumberDowngradedCustomers = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItemDowngradedCustomers = temp.totalItem;
      this.loadingIndicatorDowngradedCustomers = false;
      this.isLoadingDowngradedCustomers = false;
      if (temp.customerListResponse != null) {
        this.downgradedCustomerList = temp.customerListResponse.map((result: any) => {
          const customerModel = Customer.buildCustomer(result);
          return customerModel;
        });
      }
    });
  }

  public loadActivatedCustomers(customerRequest: FilterCustomerRequest) {
    this.loadingIndicatorActivatedCustomers = true;
    const activityRequest: FilterHistoryCustomerActivitiesRequest = {
      pageNumber: customerRequest.pageNumber,
      pageSize: customerRequest.pageSize,
      fromDateFilter: customerRequest.fromDateFilter,
      toDateFilter: customerRequest.toDateFilter
    };

    this.activityService.getHistoryActivatedCustomerActivities(activityRequest).subscribe(activitiesRes => {
      const customerCodes = activitiesRes.actionModels.map(x => x.description);

      const getCustomersByCustomerCodesRequest: GetCustomersByCustomerCodesRequest = {
        customerCodes: customerCodes
      };

      this.customerService.GetCustomersByCustomerCodes(getCustomersByCustomerCodesRequest).subscribe(customersRes => {
        this.pageNumberActivatedCustomers = activitiesRes.pageNumber;
        this.pageSize = activitiesRes.pageSize;
        this.totalItemActivatedCustomers = activitiesRes.totalItem;
        this.loadingIndicatorActivatedCustomers = false;
        this.isLoadingActivatedCustomers = false;

        if (customersRes.customerListResponse != null) {
          const customers = customersRes.customerListResponse;
          this.activatedCustomerList = activitiesRes.actionModels.map(activity => {
            const customer = customers.find(x => x.customerCode === activity.description);
            if (customer) {
              customer.activatedDate = activity.createdDate;
              const customerModel = Customer.buildCustomer(customer);
              return customerModel;
            }

            return new Customer();
          });
        }
      });
    });
  }

  public loadDeactivatedCustomers(customerRequest: FilterCustomerRequest) {
    this.loadingIndicatorDeactivatedCustomers = true;

    const activityRequest: FilterHistoryCustomerActivitiesRequest = {
      pageNumber: customerRequest.pageNumber,
      pageSize: customerRequest.pageSize,
      fromDateFilter: customerRequest.fromDateFilter,
      toDateFilter: customerRequest.toDateFilter
    };

    this.activityService.getHistoryDeactivatedCustomerActivities(activityRequest).subscribe(activitiesRes => {
      const customerCodes = activitiesRes.actionModels.map(x => x.description);
      const getCustomersByCustomerCodesRequest: GetCustomersByCustomerCodesRequest = {
        customerCodes: customerCodes
      };

      this.customerService.GetCustomersByCustomerCodes(getCustomersByCustomerCodesRequest).subscribe(customersRes => {

        this.pageNumberDeactivatedCustomers = activitiesRes.pageNumber;
        this.pageSize = activitiesRes.pageSize;
        this.totalItemDeactivatedCustomers = activitiesRes.totalItem;
        this.loadingIndicatorDeactivatedCustomers = false;
        this.isLoadingDeactivatedCustomers = false;

        if (customersRes.customerListResponse != null) {
          const customers = customersRes.customerListResponse;
          this.deactivatedCustomerList = activitiesRes.actionModels.map(activity => {
            const customer = customers.find(x => x.customerCode === activity.description);
            if (customer) {
              customer.deactivatedDate = activity.createdDate;
              const customerModel = Customer.buildCustomer(customer);
              return customerModel;
            }

            return new Customer();
          });
        }
      });
    });
  }

  setNewBasicMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadNewBasicCustomers(request);
  }

  setUpgradedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : '',
      testDate: new Date().toISOString()
    };
    this.loadUpgradedCustomers(request);
  }

  setRenewedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : '',
      testDate: new Date().toISOString()
    };
    this.loadRenewedCustomers(request);
  }

  setExtendedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : '',
      testDate: new Date().toISOString()
    };
    this.loadExtendedCustomers(request);
  }

  setExpiredMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadExpiredCustomers(request);
  }

  setDowngradedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadDowngradedCustomers(request);
  }

  setActivatedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadActivatedCustomers(request);
  }

  setDeactivatedMembersPage(pageInfo: { offset: number; }) {
    const request: FilterCustomerRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      fromDateFilter: this.fromDateRequest,
      toDateFilter: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.loadDeactivatedCustomers(request);
  }

  onClickShowComment(comment: any) {
    const dialogRef = this.modalService.open(InfoDialogComponent, { size: 'lg', centered: true, backdrop: 'static' });
    const instance = dialogRef.componentInstance;
    instance.header = 'Comment';
    instance.content = comment;
  }
}


