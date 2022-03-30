from util import auth_utils, room_utils
import random

user_id = random.randint(1, 10000000)

USER_NAME = f"user{user_id}"
SCREEN_NAME = f"Person {user_id}"


create_user_response = auth_utils.create_user(USER_NAME, SCREEN_NAME)
final_user_name = create_user_response.json()["finalUserName"]
print(final_user_name)

token = auth_utils.do_login(final_user_name)
print(token)

data_response = room_utils.get_all_rooms(token)
print(data_response)
