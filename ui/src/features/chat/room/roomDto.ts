export interface RoomDto {
  id: number;
  name: string;
}

export interface CreateNewRoomResponse {
  id: number;
  success: boolean;
  failureReason: string;
}
