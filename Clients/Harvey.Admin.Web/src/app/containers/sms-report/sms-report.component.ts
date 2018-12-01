import { Component, OnInit } from '@angular/core';
import { SMSReportService } from 'src/app/containers/sms-report/sms-report.service';
import { SMSNotificationModel, GetSMSNotificationsRequest } from 'src/app/containers/sms-report/sms-report.model';
import { ViewEncapsulation } from '@angular/core';
import { fromEvent } from 'rxjs';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-sms-report',
  templateUrl: './sms-report.component.html',
  styleUrls: ['./sms-report.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SmsReportComponent implements OnInit {

  constructor(
    private SNSReportService: SMSReportService) {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
  }

  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public smsNotificationList: Array<SMSNotificationModel> = [];
  public loadingIndicator = true;
  public isClicked = [];
  public filter_date: any;
  public dateFilter = '';
  public searchText = '';
  public widthPhoneColumn = 40;
  public widthContentColumn = 550;
  public widthTypeColumn = 200;
  @ViewChild('searchInput')
  searchInput!: ElementRef;

  ngOnInit() {
    this.onSearch();
    this.addKeyUpEventToSearchText();
  }

  public loadSMSNotifications(request: GetSMSNotificationsRequest) {
    this.SNSReportService.GetSMSNotifications(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;
      this.loadingIndicator = false;
      this.smsNotificationList = temp.listNotification.map(result => {
        const smsNotificationModel = SMSNotificationModel.buildSMSNotification(result);
        return smsNotificationModel;
      });
    });
  }

  setPage(pageInfo: { offset: number; }) {
    const request: GetSMSNotificationsRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      searchText: this.searchText,
      dateFilter: this.dateFilter
    };
    this.loadSMSNotifications(request);
  }

  onClickReset() {
    this.filter_date = undefined;
    this.searchText = '';
    this.dateFilter = '';
    this.setPage({ offset: 0 });
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

  updateFilterDate() {
    const date = new Date(this.filter_date.year, this.filter_date.month - 1, this.filter_date.day);
    this.dateFilter =  moment.utc(date).local().format('DD/MM/YYYY');
    this.onSearch();
  }
}
