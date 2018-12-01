import { UserActions, UserActionTypes } from './user.actions';

export interface UserState {
  userName: string;
  userId: string;
}

const initialState: UserState = {
  userName: null,
  userId: null
};

export function reducer(state = initialState, action: UserActions): UserState {
  switch (action.type) {
    case UserActionTypes.LoginSuccessul:
      return {
        ...state,
        userName: action.payload.name,
        userId: action.payload.sub
      };

    default:
      return state;
  }
}
