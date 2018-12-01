import { Component, OnInit } from '@angular/core';
import { Input } from '@angular/core';

@Component({
  selector: 'app-loading-indicator',
  templateUrl: './loading-indicator.component.html',
  styleUrls: ['./loading-indicator.component.scss']
})
export class LoadingIndicatorComponent implements OnInit {

  constructor() { }

  @Input()
  private _countRefresh: any;
  public get countRefresh() {
    return this._countRefresh;
  }
  public set countRefresh(value) {
    this._countRefresh = value;
  }

  ngOnInit() {
  }

}
