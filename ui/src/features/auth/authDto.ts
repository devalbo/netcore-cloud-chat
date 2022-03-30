export interface DoSignupResponse {
  success: boolean;
  finalUserName: string;
}

export interface DoLoginResponse {
  success: boolean;
  token: string;
  loggedInUsername: string;
  loggedInScreenName: string;
}

export interface DoLogoutResponse { }