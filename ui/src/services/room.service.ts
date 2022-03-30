import { TokenService } from "./token.service";

export interface APIErrorResponse {
    code: number; 
    message: string;
}

const API_URL: string = process.env.ROOM_API_ENDPOINT || 'http://localhost:5000/rooms';


export interface CreateNewRoomRequest {
  name: string;
}

export interface AuthenticateParameters {
  userName: string;
}

export interface LogoutParameters {
  jwt: string;
}

export class RoomService {

    static createRoom(createNewRoomRequest: CreateNewRoomRequest) {
      return RoomService.do(`${API_URL}/new`, {
        method: 'POST',
        headers: [
          ["Content-Type", "application/json"],
          TokenService.createTokenHeader(),
        ],
        body: JSON.stringify(createNewRoomRequest)
      });
    }

    static getAllRooms() {
      return RoomService.do(`${API_URL}`, {
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
        await RoomService.throwIfError(res);
        if (RoomService.isJson(res)) {
            return await res.json();
        }
        return await res.text();
    }

    static async throwIfError(res: Response): Promise<void> {
        if (res.ok) {
            return;
        }
        if (RoomService.isJson(res)) {
            throw await res.json();
        }
        const error: APIErrorResponse = { code: res.status, message: res.statusText };
        throw error;
    }
}
