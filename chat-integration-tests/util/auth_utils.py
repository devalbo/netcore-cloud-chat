import requests

from .config import SERVER_URL

CREATE_USER_URL = f"{SERVER_URL}/auth/create-user"
LOGIN_URL = f"{SERVER_URL}/auth/authenticate"
LOGOUT_URL = f"{SERVER_URL}/auth/logout"
DATA_URL = f"{SERVER_URL}/auth/data"


def _create_auth_headers(token):
    headers = {
        "Chat-Auth": token
    }
    return headers


def create_user(user_name, screen_name):
    create_user_response = requests.post(CREATE_USER_URL,
                                         json={
                                             "UserName": user_name,
                                             "ScreenName": screen_name
                                         },
                                         verify=False)
    return create_user_response


def do_login(user_name):
    token_response = requests.post(LOGIN_URL, json={"UserName": user_name}, verify=False)
    token = token_response.json()["token"]
    return token


def do_logout(jwt_token):
    logout_response = requests.post(LOGOUT_URL, headers=_create_auth_headers(jwt_token), verify=False)
    return logout_response


def do_get_data(jwt_token):
    data_response = requests.get(DATA_URL, headers=_create_auth_headers(jwt_token), verify=False)
    return data_response
