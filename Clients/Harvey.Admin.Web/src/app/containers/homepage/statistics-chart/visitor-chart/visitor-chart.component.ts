import { Component, OnInit, Input } from '@angular/core';
import { GetVisitorsStatisticsResponse } from '../../homepage.model';
import * as moment from 'moment';

@Component({
  selector: 'app-visitor-chart',
  templateUrl: './visitor-chart.component.html',
  styleUrls: ['./visitor-chart.component.scss']
})
export class VisitorChartComponent implements OnInit {

  @Input() getVisitorsStatisticsResponse: GetVisitorsStatisticsResponse = new GetVisitorsStatisticsResponse();

  public visitorsLineChartData: Array<any> = [
    { data: [], label: 'First-time visitors' },
    { data: [], label: 'Unique visitors' }
  ];
  public visitorsLineChartLabels: Array<any> = [];
  public visitorsLineChartOptions: any = {
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
      mode: 'label',
      callbacks: {
        label: function (tooltipItem: any, data: any) {
          const corporation = data.datasets[tooltipItem.datasetIndex].label;
          const valor = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];

          let total = 0;
          for (let i = 0; i < data.datasets.length; i++) {
            total += data.datasets[i].data[tooltipItem.index];
          }

          if (tooltipItem.datasetIndex !== data.datasets.length - 1) {
            return corporation + ': ' + valor;
          } else {
            return [corporation + ': ' + valor, 'Total visitors: ' + total];
          }
        }
      }
    }
  };
  public visitorsLineChartColors: Array<any> = [
    {
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#949FB1',
      pointHoverBorderColor: 'rgba(255,255,255,0.8)',
      tension: 0,
      fill: false,
      radius: 6
    },
    {
      backgroundColor: 'rgba(25,181,254,0.2)',
      borderColor: 'rgba(25,181,254,1)',
      pointBackgroundColor: 'rgba(25,181,254,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#19B5FE',
      pointHoverBorderColor: 'rgba(255,255,255,0.8)',
      tension: 0,
      fill: false,
      radius: 6
    }
  ];
  public visitorsLineChartLegend = true;
  public visitorsLineChartType = 'line';

  constructor() { }

  ngOnInit() {
    if (this.getVisitorsStatisticsResponse) {
      this.visitorsLineChartData[0].data = this.getVisitorsChartData(this.getVisitorsStatisticsResponse);
      this.visitorsLineChartData[1].data = this.getUniqueVisitorsChartData(this.getVisitorsStatisticsResponse);
      this.visitorsLineChartLabels = this.getVisitorsChartLabels(this.getVisitorsStatisticsResponse);
    }
  }

  getVisitorsChartData(data: GetVisitorsStatisticsResponse) {
    const listData = new Array<number>();
    data.dataVisitorsStatistic.forEach(element => {
      const value = element.value ? element.value : 0;
      listData.push(value);
    });
    return listData;
  }

  getUniqueVisitorsChartData(data: GetVisitorsStatisticsResponse) {
    const listData = new Array<number>();
    data.dataVisitorsStatistic.forEach(element => {
      const value = element.uniqueValue ? element.uniqueValue : 0;
      listData.push(value);
    });
    return listData;
  }

  getVisitorsChartLabels(data: GetVisitorsStatisticsResponse) {
    const labels = new Array<string>();
    data.dataVisitorsStatistic.forEach(element => {
      const label = element.time ? moment.utc(element.time).local().format('DD/MM/YYYY') : 'undefined';
      labels.push(label);
    });
    return labels;
  }

}
