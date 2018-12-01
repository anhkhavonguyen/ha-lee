import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromAuths from './user.reducer';

const getAuthFeatureState = createFeatureSelector<fromAuths.UserState>('auth');

export const getUserName = createSelector(
    getAuthFeatureState,
    state => state.userName
);


export const getUserId = createSelector(
    getAuthFeatureState,
    state => state.userId
);
