from . import auth_utils


class LoggedInUser:
    def __init__(self, user_name, screen_name, db_id, token):
        self.user_name = user_name
        self.screen_name = screen_name
        self.db_id = db_id
        self.token = token

    def create_headers(self):
        headers = {
            "Chat-Auth": self.token
        }
        return headers


def create_logged_in_user(user_id) -> LoggedInUser:
    user_name = f"user{user_id}"
    screen_name = f"Person {user_id}"

    created_user = auth_utils.create_user(user_name, screen_name).json()
    final_user_name = created_user["finalUserName"]
    db_id = created_user["id"]
    print(final_user_name)

    token = auth_utils.do_login(final_user_name)

    logged_in_user = LoggedInUser(final_user_name, screen_name, db_id, token)
    return logged_in_user
