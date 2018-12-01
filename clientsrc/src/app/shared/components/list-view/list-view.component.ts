import { Component, OnInit, Input, ViewChild, EventEmitter, Output, SimpleChanges, OnChanges, ElementRef } from '@angular/core';
import { AppSettingService } from '../../services/appsetting.service';
import { AppSettingModel } from '../../base-model/appsetting.model';
import { NotificationService } from '../../services/notification.service';
import { UserDefinedColumnSetting } from '../../base-model/user-defined-column-setting.model';
import { fromEvent } from 'rxjs';

@Component({
    selector: 'app-list-view',
    templateUrl: './list-view.component.html',
    styleUrls: ['./list-view.component.scss']
})
export class ListViewComponent implements OnInit, OnChanges {

    constructor(
        private appSettingService: AppSettingService,
        private notificationService: NotificationService
    ) { }

    @Input() title;
    @Input() columns;
    @Input() datasource;

    @Input() totalItems = 0;
    @Input() pageNumber = 0;
    @Input() pageSize = 10;
    @Input() userDefinedColumnSetting: UserDefinedColumnSetting;
    @Input() isHiddenSearchBox;
    countColumns = 0;
    preSelect: any;
    selected = [];
    public selectedColumns = [];
    private datasourceLabel = 'datasource';
    private userDefinedColumnSettingLabel = 'userDefinedColumnSetting';
    private keyUserConfigurationId: string;
    public searchText = '';
    @Output() sendSelectedPage = new EventEmitter<any>();
    @Output() selectedRow = new EventEmitter<any>();
    @Output() searchQuery = new EventEmitter<any>();
    @ViewChild('myTable') table: any;
    @ViewChild('searchInput')
    searchInput!: ElementRef;

    ngOnInit() {
        this.setColumns();
        this.addKeyUpEventToSearchText();
    }

    ngOnChanges(changes: SimpleChanges) {
        for (const propName in changes) {
            if (propName === this.datasourceLabel) {
                const change = changes[propName];
                if (change.currentValue && change.previousValue) {
                    const curVal = change.currentValue.length;
                    const prevVal = change.previousValue.length;
                    if (curVal !== prevVal) {
                        this.setColumns();
                    }
                }
            } else if (propName === this.userDefinedColumnSettingLabel) {
                const change = changes[propName];
                const curVal = change.currentValue;
                const prevVal = change.previousValue;
                if (curVal !== prevVal) {
                    this.getUserConfiguration();
                }
            }
        }
    }

    onChange(e: any) {
        const arrayData: Array<string> = [];
        const appsetting: AppSettingModel = {
            id: this.keyUserConfigurationId,
            key: this.userDefinedColumnSetting.key,
            value: arrayData.concat(this.selectedColumns).toString()
        };
        if (this.keyUserConfigurationId) {
            this.appSettingService.update(appsetting).subscribe(res => {
                this.notificationService.success('selected columns is updated.');
            });
        }
        else {
            this.appSettingService.add(appsetting).subscribe(res => {
                this.notificationService.success('selected columns is updated.');
            });
        }

    }

    onRemove(e: any) {
        if (this.selectedColumns.length === 0) {
            this.selectedColumns = [e.value];
        }
    }

    setPage(pageInfo: { offset: number; }) {
        this.sendSelectedPage.emit(pageInfo.offset);
    }

    onSelect({ selected }) {
        this.selectedRow.emit(selected);
    }

    setColumns() {
        if (this.datasource && this.datasource.length > 0) {
            this.columns = Object.keys(this.datasource[0]);
            if (!this.selectedColumns) {
                this.selectedColumns = this.columns;
            }
        }
    }

    getUserConfiguration() {
        if (this.userDefinedColumnSetting) {
            this.appSettingService.getBy(this.userDefinedColumnSetting.key).subscribe(
                (data: any) => {
                    if (data && data.length > 0) {
                        this.keyUserConfigurationId = data[0].id;
                        const value = data[0].value.split(',');
                        this.selectedColumns = value;
                    }
                    else {
                        this.selectedColumns = this.userDefinedColumnSetting.defaultValue.split(',');
                    }
                }
            );
        }
    }

    addKeyUpEventToSearchText() {
        fromEvent(this.searchInput.nativeElement, 'keyup')
          .subscribe(() => {
            this.searchQuery.emit(this.searchText);
          });
      }
}
