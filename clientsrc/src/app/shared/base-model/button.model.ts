import { Component } from '@angular/core/src/metadata/directives';

export class Button {
    constructor(button?: any) {
        if (!button) { return; }
        this.id = button.id;
        this.title = button.title;
        this.component = button.component;
        this.configDialog = button.configDialog;
        this.disable = button.disable;
    }
    public id = 0;
    public title = '';
    public component: Component;
    public configDialog: any;
    public disable: boolean;
}
