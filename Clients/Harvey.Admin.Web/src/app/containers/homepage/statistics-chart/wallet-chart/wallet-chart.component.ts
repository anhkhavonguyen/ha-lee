import { Component, OnInit, Input } from '@angular/core';
import { GetWalletStatisticsRequest, GetWalletStatisticsResponse } from '../../homepage.model';
import * as moment from 'moment';

@Component({
  selector: 'app-wallet-chart',
  templateUrl: './wallet-chart.component.html',
  styleUrls: ['./wallet-chart.component.scss']
})
export class WalletChartComponent implements OnInit {

  @Input() getWalletStatisticsResponse: GetWalletStatisticsResponse = new GetWalletStatisticsResponse();

  public walletLineChartData: Array<any> = [
    { data: [], label: 'Topup' },
    { data: [], label: 'Spend' }
  ];
  public walletLineChartLabels: Array<any> = [];
  public walletLineChartOptions: any = {
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true,
        }
      }]
    },
    legend: {
      position: 'right'
    },
    tooltips: {
      mode: 'x'
    }
  };
  public walletLineChartColors: Array<any> = [
    {
      backgroundColor: 'rgba(40,167,69,0.2)',
      borderColor: 'rgba(40,167,69,1)',
      pointBackgroundColor: 'rgba(40,167,69,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#28A745',
      pointHoverBorderColor: 'rgba(255,255,255,0.8)',
      tension: 0,
      fill: false,
      radius: 6
    },
    {
      backgroundColor: 'rgba(221, 44, 44, 0.2)',
      borderColor: 'rgba(221, 44, 44, 1)',
      pointBackgroundColor: 'rgba(221, 44, 44, 1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#DD2C2C',
      pointHoverBorderColor: 'rgba(255,255,255,0.8)',
      tension: 0,
      fill: false,
      radius: 6
    }
  ];
  public walletLineChartLegend = true;
  public walletLineChartType = 'line';

  constructor() { }

  ngOnInit() {
    if (this.getWalletStatisticsResponse) {
      this.walletLineChartData[0].data = this.getTopupChartData(this.getWalletStatisticsResponse);
      this.walletLineChartData[1].data = this.getSpendChartData(this.getWalletStatisticsResponse);
      this.walletLineChartLabels = this.getVisitorsChartLabels(this.getWalletStatisticsResponse);
    }
  }

  getTopupChartData(data: GetWalletStatisticsResponse) {
    const listData = new Array<number>();
    data.dataWalletStatistics.forEach(element => {
      const value = element.totalTopup ? element.totalTopup : 0;
      listData.push(value);
    });
    return listData;
  }

  getSpendChartData(data: GetWalletStatisticsResponse) {
    const listData = new Array<number>();
    data.dataWalletStatistics.forEach(element => {
      const value = element.totalSpend ? element.totalSpend : 0;
      listData.push(value);
    });
    return listData;
  }

  getVisitorsChartLabels(data: GetWalletStatisticsResponse) {
    const labels = new Array<string>();
    data.dataWalletStatistics.forEach(element => {
      const label = element.time ? moment.utc(element.time).local().format('DD/MM/YYYY') : 'undefined';
      labels.push(label);
    });
    return labels;
  }

}
