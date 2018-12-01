import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit {

  @Input() countRefresh;
  @Output() retry = new EventEmitter<boolean>();

  constructor() {
  }

  ngOnInit() {
  }

  onButtonRetryClick() {
    this.retry.emit(true);
  }

  onButtonRetryBack() {
    this.retry.emit(false);
  }

}
