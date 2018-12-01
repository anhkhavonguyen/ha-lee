import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ServiceBase } from './service-base';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';
import { ChannelModel } from 'src/app/ims/channels/channel.model';

@Injectable({ providedIn: 'root' })
export class ChannelService extends ServiceBase {
  getAll(page: number = 1, numberItemsPerPage: number = 10) {
    return this.page(`${environment.apiUrl}/api/channels`, new PagingFilterCriteria(page, numberItemsPerPage));
  }

  getBy(id: string): Observable<ChannelModel> {
    return this.get(`${environment.apiUrl}/api/channels/${id}`);
  }

  add(channel: ChannelModel): Observable<ChannelModel> {
    return this.post(`${environment.apiUrl}/api/channels`, channel);
  }

  update(channel: ChannelModel): Observable<ChannelModel> {
    return this.put(`${environment.apiUrl}/api/channels/${channel.id}`, channel);
  }

  remove(id: string) {
    return this.delete(`${environment.apiUrl}/api/channels/${id}`);
  }

  updateProvision(channelId: string): Observable<any> {
    return this.post(`${environment.apiUrl}/api/channels/${channelId}/provision`);
  }

  getCatalog(channelId: string): Observable<any> {
    return this.get(`${environment.apiUrl}/api/channels/${channelId}/products`);
  }
}
