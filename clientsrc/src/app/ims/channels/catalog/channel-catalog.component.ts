import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { ChannelService } from 'src/app/shared/services/channel.service';
import { Store, select } from '@ngrx/store';
import * as fromChannel from '../state/channel.reducer';
import * as channelSelector from '../state/index';
import { takeWhile } from 'rxjs/operators';
import { CatalogModel } from './channel-catalog.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-catalog-channel',
    templateUrl: './channel-catalog.component.html',
    styleUrls: ['./channel-catalog.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class ChannelCatalogComponent extends ComponentBase {
    private componentActive = true;
    public channelId: string;
    public datasource: any[] = [];
    @ViewChild('catalogTable') table: any;
    fieldValues: any;
    columnCount = 0;

    public filterData: CatalogModel[] = [];

    constructor(private channelService: ChannelService,
        private store: Store<fromChannel.ChannelState>,
        private activeModal: NgbActiveModal) {
        super();
    }

    onInit() {
        this.handleSubscription(this.store.pipe(
            select(channelSelector.getSelectedItem), takeWhile(() => this.componentActive))
            .subscribe(
                (id: string) => {
                    if (id == null) {
                        return;
                    }
                    this.getCata(id);
                }
            ));
    }
    onDestroy() {
    }

    onClose(): void {
        this.activeModal.close('closed');
    }

    onDismiss(reason: String): void {
        this.activeModal.dismiss(reason);
    }

    getCata(channelId) {
        this.channelService.getCatalog(channelId).subscribe(result => {
            this.datasource = result;
        });
    }

    filter(products: any) {
        this.filterData = [];
        products.forEach(product => {
            product.variants.forEach(variant => {
                if (variant) {
                    this.fieldValues = variant.fields;
                    this.columnCount = this.fieldValues.length;
                }
            });
        });
    }

    toggleExpandRow(row) {
        this.table.rowDetail.toggleExpandRow(row);
    }

    onDetailToggle(event) {
    }

    getFieldValue(variants: any) {
        if (variants && variants.length > 0) {
            return variants[0].fields;
        }
    }

    getColumnCount(fields: any) {
        if (fields) {
            return fields.length;
        }
    }
}
