import { Component } from '@angular/core';
import { FieldBaseComponent } from '../field-base/field-base.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ListEntityReferenceComponent } from '../list-entity-reference/list-entity-reference.component';
import { ReferenceEntityValue } from '../field-base/field-value';

@Component({
    selector: 'app-entity-reference-field',
    templateUrl: './entity-reference-field.component.html',
    styleUrls: ['./entity-reference-field.component.scss']
})

export class EntityRefenceFieldComponent extends FieldBaseComponent<string[]> {

    GetValue(value: any): string[] {
        return value;
    }
    Validate(): boolean {
        return true;
    }
    constructor(private modalService: NgbModal) {
        super();
    }

    public openModal() {
        const dialogRef = this.modalService.open(ListEntityReferenceComponent, { size: 'lg', centered: true, backdrop: 'static' });
        const instance = dialogRef.componentInstance;
        instance.dataSource = this.data;
        return dialogRef.result.then((result) => {
            this.bindingValue = result.selected[0];
            this.save();
          }, (reason) => {
          });
    }
}
