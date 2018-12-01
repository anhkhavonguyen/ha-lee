import { Component, OnInit, Input } from '@angular/core';
import { FieldBaseComponent } from '../field-base/field-base.component';

@Component({
    selector: 'app-dropdown-list-field',
    templateUrl: './dropdown-list-field.component.html',
    styleUrls: ['./dropdown-list-field.component.scss']
})

export class DropdownListFieldComponent extends FieldBaseComponent<string[]> implements OnInit {

    @Input() isModeEdit;

    GetValue(value: any): string[] {
        return value;
    }
    Validate(): boolean {
        return true;
    }
    constructor() {
        super();
    }

}
