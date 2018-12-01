import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PageMessageService {
  private message = new Subject<string>();

  constructor() {
  }

  sendMessageContent(messages: string) {
    this.message.next(messages);
  }

  getMessage(): Observable<string> {
    return this.message.asObservable();
  }
}
