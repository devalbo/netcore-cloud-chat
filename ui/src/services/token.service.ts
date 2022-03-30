export class TokenService {

  static CHAT_AUTH_KEY = 'chat-auth';

  static saveAuthToken(token: string) {
    // there are security ramifications for putting this in localStorage, but there's only so much time in the day 
    // and I don't have a chance to refactor my store :(
    window.localStorage.setItem(TokenService.CHAT_AUTH_KEY, token);
  }

  static hasAuthToken(): boolean {
    const token = window.localStorage.getItem(TokenService.CHAT_AUTH_KEY);
    return token !== null;
  }

  static getAuthToken(): string {
    const token = window.localStorage.getItem(TokenService.CHAT_AUTH_KEY);
    if (token === null) {
      throw new Error("No token available");
    }
    return token;
  }

  static clearAuthToken() {
    window.localStorage.removeItem(TokenService.CHAT_AUTH_KEY);
  }

  static createTokenHeader(): string[] {
    const token = this.getAuthToken();
    return ["Chat-Auth", token];
  }
}