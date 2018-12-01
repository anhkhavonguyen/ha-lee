import { Component, OnInit } from '@angular/core';
import { ErrorLogRequest, ErrorLogEntry } from 'src/app/containers/error-log/error-log.model';
import { ErrorLogService } from 'src/app/containers/error-log/error-log.service';
import { ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-error-log',
  templateUrl: './error-log.component.html',
  styleUrls: ['./error-log.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ErrorLogComponent implements OnInit {

  constructor(private errorLogService: ErrorLogService) {
    this.pageNumber = 0;
    this.pageSize = 10;
    this.totalItem = 0;
   }

  public pageNumber: number;
  public pageSize: number;
  public totalItem: number;
  public listError: Array<ErrorLogEntry> = [];
  public loadingIndicator = true;

  ngOnInit() {
    this.setPage({ offset: 0 });
  }

  public loadErrorsLog(request: ErrorLogRequest) {
    this.errorLogService.GetErrorLog(request).subscribe(res => {
      const temp = res;
      this.pageNumber = temp.pageNumber;
      this.pageSize = temp.pageSize;
      this.totalItem = temp.totalItem;
      this.loadingIndicator = false;
      this.listError = temp.listError.map(result => {
        const errorEntryModel = ErrorLogEntry.buildLogEntry(result);
        return errorEntryModel;
      });
    });
  }

  setPage(pageInfo: { offset: number; }) {
    const request: ErrorLogRequest = {
      pageNumber: pageInfo.offset,
      pageSize: this.pageSize
    };
    this.loadErrorsLog(request);
  }
}
