from util import user_utils, message_utils, room_utils
import random

user1_id = random.randint(1, 10000000)
user2_id = user1_id + 1
user3_id = user2_id + 1

ROOM_PREFIX = f"rm{user1_id}:"


user1 = user_utils.create_logged_in_user(user1_id)
user2 = user_utils.create_logged_in_user(user2_id)
user3 = user_utils.create_logged_in_user(user3_id)

room1 = room_utils.as_user_create_room(user1, ROOM_PREFIX + "1")

message_utils.as_user_add_message(user1, room1, "Hi there Room 1")

room2 = room_utils.as_user_create_room(user2, ROOM_PREFIX + "2")
room3 = room_utils.as_user_create_room(user3, ROOM_PREFIX + "3")
room4 = room_utils.as_user_create_room(user3, ROOM_PREFIX + "4")

message_utils.as_user_add_message(user3, room2, "Hi there Room 2")
message_utils.as_user_add_message(user1, room1, "Hi there again Room 1")

all_rooms = room_utils.as_user_get_all_rooms(user1)
print(all_rooms)
