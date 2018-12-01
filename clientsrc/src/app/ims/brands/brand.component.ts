import { Component, ViewEncapsulation } from '@angular/core';
import { ActionType } from 'src/app/shared/constant/action-type.constant';
import { Store, select } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';
import { Button } from 'src/app/shared/base-model/button.model';
import * as fromBrand from '../brands/state/brand.reducer';
import * as brandActions from '../brands/state/brand.action';
import * as brandSelector from '../brands/state/index';
import * as fromAuths from '../../shared/components/auth/state/index';
import { BrandModel } from './brand.model';
import { AddBrandComponent } from './add-brand/add-brand.component';
import { UpdateBrandComponent } from './update-brand/update-brand.component';
import { DeleteBrandComponent } from './delete-brand/delete-brand.component';
import { ComponentBase } from 'src/app/shared/components/component-base';
import { UserDefinedColumnSetting } from 'src/app/shared/base-model/user-defined-column-setting.model';
import { PagingFilterCriteria } from 'src/app/shared/base-model/paging-filter-criteria';

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html',
  styleUrls: ['./brand.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BrandComponent extends ComponentBase {

  constructor(
    private store: Store<fromBrand.BrandState>) {
    super();
  }
  public pageSize = 10;
  public addSuccessMessage: string = 'Brand is added.';
  public updateSuccessMessage: string = 'Brand is updated.';
  public deleteSuccessMessage: string = 'Brand is deleted.';

  public componentActive = true;
  public datasource: Array<BrandModel> = [];
  public listButton;
  public title;
  public actionType = ActionType.dialog;
  public selectedId = null;
  public userDefinedColumnSetting: UserDefinedColumnSetting;
  isHiddenSearchBox = true;

  onInit() {
    this.title = 'Brand Management';
    this.handleSubscription(this.store.pipe(
      select(fromAuths.getUserId), takeWhile(() => this.componentActive))
      .subscribe(
        (id: string) => {
          if (id == null) {
            return;
          }
          this.userDefinedColumnSetting = new UserDefinedColumnSetting(`${id}_UserDefinedColumnsBrand`, 'name,description');
          this.getBrands();
          this.setButtonsConfiguration();
        }
      ));

    this.handleSubscription(this.store.pipe(
      select(brandSelector.getSelectedItem), takeWhile(() => this.componentActive))
      .subscribe(
        (id: string | null) => {
          if (id) {
            this.changeListButton(false);
          } else {
            this.changeListButton(true);
          }
        }
      ));
  }

  onDestroy() {
  }

  getBrands() {
    this.store.dispatch(new brandActions.GetBrands(new PagingFilterCriteria(1, this.pageSize)));
    this.store.pipe(select(brandSelector.getBrands),
      takeWhile(() => this.componentActive))
      .subscribe(
        (brands: Array<BrandModel>) => {
          this.datasource = brands;
        }
      );
  }

  changeSelectedPage(page: number) {
    this.store.dispatch(new brandActions.GetBrands(new PagingFilterCriteria(page + 1, this.pageSize)));
  }

  setButtonsConfiguration() {
    this.listButton = [
      new Button({
        id: 0,
        title: 'Add',
        component: AddBrandComponent,
        configDialog: { size: 'lg', centered: true, backdrop: 'static' },
      }),
      new Button({
        id: 1,
        title: 'Edit',
        component: UpdateBrandComponent,
        configDialog: { size: 'lg', centered: true, backdrop: 'static' },
        disable: true
      }),
      new Button({
        id: 2,
        title: 'Delete',
        component: DeleteBrandComponent,
        configDialog: { size: 'lg', centered: true, backdrop: 'static' },
        disable: true
      })
    ];
  }


  changeListButton(isDisabled: boolean) {
    if (!this.listButton) {
      return;
    }
    this.listButton.forEach(btn => {
      if (btn.title === 'Edit' || btn.title === 'Delete') {
        btn.disable = isDisabled;
      }
    });
  }
}