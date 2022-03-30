import { TokenService } from "./token.service";

export interface APIErrorResponse {
    code: number; 
    message: string;
}

const API_URL: string = process.env.AUTH_API_ENDPOINT || 'http://localhost:5000/auth';


export interface CreateUserParameters {
  userName: string;
  screenName: string;
}

export interface AuthenticateParameters {
  userName: string;
}

export interface LogoutParameters {
  jwt: string;
}

export class AuthService {

    static createUser(createUserParameters: CreateUserParameters) {
      return AuthService.do(`${API_URL}/create-user`, {
        method: 'POST',
        headers: [
          ["Content-Type", "application/json"]
        ],
        body: JSON.stringify({
          username: createUserParameters.userName,
          screenName: createUserParameters.screenName
        })
      });
    }

    static authenticate(authenticateParameters: AuthenticateParameters) {
      return AuthService.do(`${API_URL}/authenticate`, {
        method: 'POST',
        headers: [
          ["Content-Type", "application/json"]
        ],
        body: JSON.stringify({
          username: authenticateParameters.userName
        })
      });
    }

    static logout() {
      return AuthService.do(`${API_URL}/logout`, {
        method: 'POST',
        headers: [
          ["Content-Type", "application/json"],
          TokenService.createTokenHeader(),
        ],
      });
    }

    static isJson(res: Response): boolean {
        return !!res.headers.get('content-type')?.includes('application/json');
    }

    static async do(route: RequestInfo, options: RequestInit) {
        const res = await fetch(route, options);
        await AuthService.throwIfError(res);
        if (AuthService.isJson(res)) {
            return await res.json();
        }
        return await res.text();
    }

    static async throwIfError(res: Response): Promise<void> {
        if (res.ok) {
            return;
        }
        if (AuthService.isJson(res)) {
            throw await res.json();
        }
        const error: APIErrorResponse = { code: res.status, message: res.statusText };
        throw error;
    }
}
