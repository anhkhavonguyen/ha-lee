import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { NotificationService } from './notification.service';
import { PagedResult } from '../base-model/paged-result';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';

@Injectable()
export abstract class ServiceBase {
  constructor(
    private http: HttpClient,
    private notificationService: NotificationService
  ) { }
  protected post<T>(url: string, data?: T): Observable<T> {
    return this.http
      .post<T>(url, data)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected put<T>(url: string, data: T): Observable<T> {
    return this.http
      .put<T>(url, data)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected delete(url: string) {
    return this.http
      .delete<boolean>(url)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected get<T>(url: string): Observable<T> {
    return this.http
      .get<T>(url)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected list<T>(url: string) {
    return this.http
      .get<T[]>(url)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected page<T>(url: string, pagingFilterCriteria: PagingFilterCriteria) {
    return this.http
      .get<PagedResult<T>>(`${url}?page=${pagingFilterCriteria.page}&numberItemsPerPage=${pagingFilterCriteria.numberItemsPerPage}`)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  protected pageWithSearchText<T>(url: string, pagingFilterCriteria: PagingFilterCriteria, queryText: string) {
    return this.http
      .get<PagedResult<T>>(`${url}?page=${pagingFilterCriteria.page}&numberItemsPerPage=${pagingFilterCriteria.numberItemsPerPage}&queryText=${queryText}`)
      .pipe(catchError(err => this.handleError(err, this.notificationService)));
  }

  private handleError(err, notificationService: NotificationService) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      errorMessage = err.error.detail;
    }
    notificationService.error(errorMessage);
    return throwError(errorMessage);
  }
}
