import requests

from .config import SERVER_URL
from .user_utils import LoggedInUser

ALL_ROOMS_URL = f"{SERVER_URL}/rooms"
NEW_ROOM_URL = f"{SERVER_URL}/rooms/new"


class Room:
    def __init__(self, room_id):
        self.room_id = room_id


def get_all_rooms(token: str):
    data_response = requests.get(ALL_ROOMS_URL, headers = {"Chat-Auth": token}, verify=False)
    return data_response.json()


def as_user_get_all_rooms(user: LoggedInUser):
    data_response = requests.get(ALL_ROOMS_URL, headers=user.create_headers(), verify=False)
    return data_response.json()


def as_user_create_room(user: LoggedInUser, room_name: str):
    data_response = requests.post(NEW_ROOM_URL,
                                  headers=user.create_headers(),
                                  json={"name": room_name},
                                  verify=False)
    response_json = data_response.json()
    success = response_json["success"]
    if not success:
        raise Exception(f"Could not create room: {response_json['failureReason']}")

    room_id = response_json["id"]
    room = Room(room_id)

    return room
