import { FieldModel } from '../fields/field.model';
import { FieldValue } from '../fields/field-base/field-value';

export class FieldTemplateModel {
    constructor(item: any) {
        this.id = item.id;
        this.name = item.name;
        this.description = item.description;
        this.sections = item.sections;
        this.type = item.type;
    }
    id: string;
    name: string;
    description: string;
    sections: Section[];
    type: string;
}

export class FieldFieldTemplateModel {
    constructor(item: any) {
        this.field = new FieldModel(item.field);
    }
    field: FieldModel;
    action: string;
}

export class Section {
    constructor(item: any) {
        this.name = item.name;
        this.isVariantField = item.isVariantField;
        this.orderSection = item.orderSection;
        this.fields = item.fields;
    }
    name: string;
    isVariantField: boolean;
    orderSection: number;
    fields: FieldFieldTemplateModel[];
}

export enum FieldTemplateType {
    WithVariant = 1,
    WithoutVariant = 2
}

export const FieldTemplateTypes = {
    'With Variant': FieldTemplateType.WithVariant,
    'Without Variant': FieldTemplateType.WithoutVariant
};

export enum FieldFieldTemplateAction {
    Get = 0,
    Add = 1,
    Update = 2,
    Delete = 3,
}

export const FieldFieldTemplateActions = {
    Get: FieldFieldTemplateAction.Get,
    Add: FieldFieldTemplateAction.Add,
    Update: FieldFieldTemplateAction.Update,
    Delete: FieldFieldTemplateAction.Delete
};


