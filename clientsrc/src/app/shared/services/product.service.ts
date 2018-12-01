import { ServiceBase } from './service-base';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProductModel, ProductModelRequest, ProductListModel } from 'src/app/ims/products/product';
import { PagingFilterCriteria } from '../base-model/paging-filter-criteria';

@Injectable({ providedIn: 'root' })
export class ProductService extends ServiceBase {

    getAllWithoutPaging() {
        return this.list<ProductListModel>(`${environment.apiUrl}/api/products/all`);
    }

    getAll(page: number = 1, numberItemsPerPage: number = 10, queryText = '') {
        return this.pageWithSearchText(`${environment.apiUrl}/api/products`, new PagingFilterCriteria(page, numberItemsPerPage), queryText);
    }

    createProductFromTemplate(templateId: string) {
        return this.get<ProductModel>(`${environment.apiUrl}/api/products/template/${templateId}`);
    }

    getById(id: string) {
        return this.get<ProductModel>(`${environment.apiUrl}/api/products/${id}`);
    }

    add(product: ProductModelRequest) {
        return this.post<ProductListModel>(`${environment.apiUrl}/api/products`, product);
    }
    update(product: ProductModelRequest) {
        return this.put(`${environment.apiUrl}/api/products/${product.id}`, product);
    }
}
