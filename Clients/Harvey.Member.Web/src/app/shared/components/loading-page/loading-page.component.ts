import {Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-loading-page',
  templateUrl: './loading-page.component.html',
  styleUrls: ['./loading-page.component.scss']
})
export class LoadingPageComponent implements OnInit {

  @Input()
  private _countRefresh: any;

  constructor() {
  }

  public get countRefresh() {
    return this._countRefresh;
  }
  public set countRefresh(value) {
    this._countRefresh = value;
  }

  ngOnInit() {
  }

}
