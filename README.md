# .Net chat app sample

This is a complete single page React/Redux application with a .NET ASP server backend written in C# with the option for a sqlite or Postgres database.

## Getting Started

### Prerequisites

- Make sure to have [docker](https://docs.docker.com/get-docker/) installed beforehand

### Running app

```shell
docker-compose up

# or run in daemon mode

docker-compose up -d
```

The first run will also build the containers.
The API should be available at `http://localhost:5000`
The UI should be available at `http://localhost:3000`

There are also some Python test scripts to help with making a more "lively" environment in `chat-integration-tests`. You can run them against the API as a sanity check and/or to populate the environment with some data. Note that the server URL can be configured in the `util/config.py` file.

If you run the API via Visual Studio/locally (not in Docker), a Swagger page will be available for direct API manipulation and testing as well.


### Note about UI

In the header there is a `ping/pong` recurring event as a check that the communication between the UI and API is working.

### Stopping app
```shell
docker-compose down -v
```
Note: the `-v` is important because of the named volume defined for the UI project.
