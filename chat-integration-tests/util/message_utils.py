import requests

from .config import SERVER_URL

NEW_MESSAGE_URL = f"{SERVER_URL}/messages/new"


def as_user_add_message(user, room, message):
    requests.post(NEW_MESSAGE_URL,
                  headers=user.create_headers(),
                  json={
                      "roomId": room.room_id,
                      "contents": message
                  },
                  verify=False)
