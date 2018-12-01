import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AssignmentState, key } from './entity-assignment.reducer';

let featureName = '';

switch (window.location.pathname) {
    case '/assortments':
        featureName = 'assortment';
        break;
    case '/channels':
        featureName = 'channel';
        break;
}

const getAssignmentFeature = createFeatureSelector<AssignmentState>(`${featureName}_assignment`);

export const getSearchAssignment = createSelector(
    getAssignmentFeature,
    state => state[key].searchAssignments
);

export const getSelectAssignment = createSelector(
    getAssignmentFeature,
    state => state[key].selectAssignments
);
