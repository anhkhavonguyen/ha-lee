import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ListViewManagementState, key } from './list-view-management.reducer';

let featureName = '';

switch (window.location.pathname) {
  case '/products':
    featureName = 'products';
    break;
  case '/assortments':
    featureName = 'assortments';
    break;
  case '/categories':
    featureName = 'categories';
    break;
  case '/fields':
    featureName = 'fields';
    break;
  case '/field-templates':
    featureName = 'fieldtemplates';
    break;
  case '/locations':
    featureName = 'locations';
    break;
  case '/brands':
    featureName = 'brands';
    break;
  case '/channels':
    featureName = 'channels';
    break;
  case '/activities':
    featureName = 'activities';
    break;
  default:
    featureName = 'products';
}

const getListViewManagementFeature = createFeatureSelector<
  ListViewManagementState
>(`${featureName}`);

export const getFormState = createSelector(
  getListViewManagementFeature,
  state => state[key].formState
);

export const getTotalItems = createSelector(
  getListViewManagementFeature,
  state => state[key].totalItems
);

export const getSelectedPage = createSelector(
  getListViewManagementFeature,
  state => state[key].selectedPage
);
