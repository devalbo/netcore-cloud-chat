import { TokenService } from "./token.service";

export interface APIErrorResponse {
    code: number; 
    message: string;
}

const API_URL: string = process.env.MESSAGE_API_ENDPOINT || 'http://localhost:5000/messages';


export interface CreateNewMessageRequest {
  contents: string;
  roomId: number;
}

export interface AuthenticateParameters {
  userName: string;
}

export interface LogoutParameters {
  jwt: string;
}

export class MessageService {

    static sendMessage(roomId: string, message: string) {
      const createNewMessageRequest: CreateNewMessageRequest = { 
        contents: message,
        roomId: parseInt(roomId)
      };

      return MessageService.do(`${API_URL}/new`, {
        method: 'POST',
        headers: [
          ["Content-Type", "application/json"],
          TokenService.createTokenHeader(),
        ],
        body: JSON.stringify(createNewMessageRequest)
      });
    }

    static getMessagesForRoom(roomId: string) {
      console.log("GETTING ");
      return MessageService.do(`${API_URL}/room/${roomId}`, {
        method: 'GET',
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
        await MessageService.throwIfError(res);
        if (MessageService.isJson(res)) {
            return await res.json();
        }
        return await res.text();
    }

    static async throwIfError(res: Response): Promise<void> {
        if (res.ok) {
            return;
        }
        if (MessageService.isJson(res)) {
            throw await res.json();
        }
        const error: APIErrorResponse = { code: res.status, message: res.statusText };
        throw error;
    }
}
