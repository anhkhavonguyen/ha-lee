import { Component, OnInit } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { fromEvent } from 'rxjs';
import { ActivityService } from './activity.service';
import { ActivitiesRequest, Activity } from './activity.model';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ActivityComponent implements OnInit {

  constructor(
    private activityService: ActivityService) {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
  }

  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public dataList: Array<Activity> = [];
  public loadingIndicator = true;
  public searchText = '';
  @ViewChild('searchInput')
  searchInput!: ElementRef;

  ngOnInit() {
    this.onSearch();
    this.addKeyUpEventToSearchText();
  }

  public loadData(request: ActivitiesRequest) {
    this.activityService.getActivities(request).subscribe(res => {
      if (res) {
        const temp = res;
        this.pageNumber = temp.pageNumber;
        this.pageSize = temp.pageSize;
        this.totalItem = temp.totalItem;
        this.loadingIndicator = false;
        this.dataList = temp.actionActivityModels.map(result => {
          const model = Activity.buildActivity(result);
          return model;
        });
      }
    });
  }

  setPage(pageInfo: { offset: number; }) {
    const request: ActivitiesRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize,
      searchText: this.searchText,
      dateFilter: ''
    };
    this.loadData(request);
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
}
