import { Component, OnInit, Input } from '@angular/core';
import { HomepageService } from '../homepage.service';
import { Subject } from 'rxjs';
import {
  GetVisitorsStatisticsRequest,
  GetVisitorsStatisticsResponse,
  GetPointsStatisticsRequest,
  GetPointsStatisticsResponse,
  GetWalletStatisticsRequest,
  GetWalletStatisticsResponse
} from '../homepage.model';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';

@Component({
  selector: 'app-statistics-chart',
  templateUrl: './statistics-chart.component.html',
  styleUrls: ['./statistics-chart.component.scss']
})
export class StatisticsChartComponent implements OnInit {

  @Input() eventUpdateStatisticsData: Subject<any> = new Subject();
  @Input() fromDateRequest: any;
  @Input() toDateRequest: any;
  @Input() outletRequest = undefined;

  public getVisitorsStatisticsResponse: GetVisitorsStatisticsResponse = new GetVisitorsStatisticsResponse();
  public getPointsStatisticsResponse: GetPointsStatisticsResponse = new GetPointsStatisticsResponse();
  public getWalletStatisticsResponse: GetWalletStatisticsResponse = new GetWalletStatisticsResponse();

  public isLoadVisitorLineChart = true;
  public isLoadPointsLineChart = true;
  public isLoadWalletLineChart = true;

  constructor(private homepageService: HomepageService,
    private translate: TranslateService,
    private toast: ToastrService) { }

  ngOnInit() {
    this.loadDataVisitorLineChart();
    this.loadDataPointsLineChart();
    this.loadDataWalletLineChart();
    this.eventUpdateStatisticsData.subscribe(event => {
      this.loadDataVisitorLineChart();
      this.loadDataPointsLineChart();
      this.loadDataWalletLineChart();
    });
  }

  loadDataVisitorLineChart() {
    this.isLoadVisitorLineChart = true;
    const request: GetVisitorsStatisticsRequest = {
      fromDate: this.fromDateRequest,
      toDate: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.homepageService.GetVisitorsStatistics(request).subscribe(res => {
      if (res && res.dataVisitorsStatistic.length) {
        this.getVisitorsStatisticsResponse = res;
        this.isLoadVisitorLineChart = false;
      }
    }, error => {
      this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(message => {
        this.toast.warning(message);
        this.isLoadVisitorLineChart = false;
      });
    });
  }

  loadDataPointsLineChart() {
    this.isLoadPointsLineChart = true;
    const request: GetPointsStatisticsRequest = {
      fromDate: this.fromDateRequest,
      toDate: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.homepageService.GetPointsStatistics(request).subscribe(res => {
      if (res && res.dataPointsStatistics.length) {
        this.getPointsStatisticsResponse = res;
        this.isLoadPointsLineChart = false;
      }
    }, error => {
      this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(message => {
        this.toast.warning(message);
        this.isLoadPointsLineChart = false;
      });
    });
  }

  loadDataWalletLineChart() {
    this.isLoadWalletLineChart = true;
    const request: GetWalletStatisticsRequest = {
      fromDate: this.fromDateRequest,
      toDate: this.toDateRequest,
      outletId: this.outletRequest ? this.outletRequest : ''
    };
    this.homepageService.GetWalletStatistics(request).subscribe(res => {
      if (res && res.dataWalletStatistics.length) {
        this.getWalletStatisticsResponse = res;
        this.isLoadWalletLineChart = false;
      }
    }, error => {
      this.translate.get('APP.ERROR.GENERAL_ERROR').subscribe(message => {
        this.toast.warning(message);
        this.isLoadWalletLineChart = false;
      });
    });
  }

}
