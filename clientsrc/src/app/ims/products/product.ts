import { FieldValue } from '../fields/field-base/field-value';
import { CategoryModel } from '../categories/category.model';


export interface ProductModel {
    id: string;
    name: string;
    description: string;
    sections: Section[];
    variants: VariantModel[];
}

export interface Section {
    name: string;
    orderSection: number;
    isVariantField: boolean;
    fieldValues: FieldValue<any>[];
}

export interface ProductListModel {
    id: string;
    name: string;
    description: string;
}

export interface ProductModelRequest {
    id: string;
    name: string;
    description: string;
    fieldTemplateId: string;
    productFields: FieldValueModel[];
    variants: VariantModelRequest[];
    categoryId: string;
}

export interface EditProductModel {
    id: string;
    name: string;
    description: string;
    fieldTemplateId: string;
    fieldTemplateName: string;
    categoryId: string;
    sections: Section[];
    variants: VariantModel[];
}

export interface FieldValueModel {
    id: string;
    fieldId: string;
    fieldValue: string;
}


export interface Product {
    id: string;
    fields: FieldValue<any>[];
}

export interface VariantModelRequest {
    variantFields: FieldValueModel[];
    prices: PriceModel;
}

export interface PriceModel {
    listPrice: number;
    staffPrice: number;
    memberPrice: number;
}

export interface VariantModel {
    id: string;
    fieldValues: FieldValue<any>[];
    price: PriceModel;
    name: string;
    orderSection: number;
    isVariantField: boolean;
}

export enum PriceEnum {
    listPrice,
    staffPrice,
    memberPrice
}

export const PriceEnums = {
    listPrice: PriceEnum.listPrice,
    staffPrice: PriceEnum.staffPrice,
    memberPrice: PriceEnum.memberPrice
}

