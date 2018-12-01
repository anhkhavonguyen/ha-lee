import { Component, OnInit, Input } from '@angular/core';
import { GetPointsStatisticsResponse } from '../../homepage.model';
import * as moment from 'moment';

@Component({
  selector: 'app-point-chart',
  templateUrl: './point-chart.component.html',
  styleUrls: ['./point-chart.component.scss']
})
export class PointChartComponent implements OnInit {

  @Input() getPointsStatisticsResponse: GetPointsStatisticsResponse = new GetPointsStatisticsResponse();

  public pointsLineChartData: Array<any> = [
    { data: [], label: 'Add' },
    { data: [], label: 'Redeem' }
  ];
  public pointsLineChartLabels: Array<any> = [];
  public pointsLineChartOptions: any = {
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
  public pointsLineChartColors: Array<any> = [
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
  public pointsLineChartLegend = true;
  public pointsLineChartType = 'line';

  constructor() { }

  ngOnInit() {
    this.pointsLineChartData[0].data = this.getAddPointsChartData(this.getPointsStatisticsResponse);
    this.pointsLineChartData[1].data = this.getRedeemPointsChartData(this.getPointsStatisticsResponse);
    this.pointsLineChartLabels = this.getPointsChartLabels(this.getPointsStatisticsResponse);
  }

  getAddPointsChartData(data: GetPointsStatisticsResponse) {
    const listData = new Array<number>();
    data.dataPointsStatistics.forEach(element => {
      const value = element.totalAdd ? element.totalAdd : 0;
      listData.push(value);
    });
    return listData;
  }

  getRedeemPointsChartData(data: GetPointsStatisticsResponse) {
    const listData = new Array<number>();
    data.dataPointsStatistics.forEach(element => {
      const value = element.totalRedeem ? element.totalRedeem : 0;
      listData.push(value);
    });
    return listData;
  }

  getPointsChartLabels(data: GetPointsStatisticsResponse) {
    const labels = new Array<string>();
    data.dataPointsStatistics.forEach(element => {
      const value = element.time ? moment.utc(element.time).local().format('DD/MM/YYYY') : 'undefined';
      labels.push(value);
    });
    return labels;
  }

}
