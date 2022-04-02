# .Net Single Page App starter

This is a complete single page React/Redux chat application with a .NET ASP server backend written in C# (ASP .NET) with the out of the box ability to configure a sqlite or Postgres database using the OrmLite library(more options are available without much effort).

It's suitable to use as a template/starting point if you want to write a C# server with a Typescript/React/Redux frontend and database for persistence. It should help you get your ideas on the screen quickly to see if they have any legs.

Next steps are:
* integrate some units tests for `ChatBackend` with xUnit, NSubstitute/AutoFixture, FluentAssertions
* add better support for loading settings from configurations rather than inline using `Microsoft.Extensions.Configuration` and `Microsoft.Extensions.Options` extensions
* clean up patterns in `ui` project, then add unit tests

Nexter steps are:
* shared type definitions between backend and frontend a la Protocol Buffers or Twirp
* more integration tests to support "business logic" type workflows continue to work even in the face of radical implementation/architecture changes


## Getting Started

### Prerequisites

- Make sure to have [docker](https://docs.docker.com/get-docker/) installed beforehand


### Running apps in docker compose

```shell
docker-compose up

# or run in daemon mode

docker-compose up -d
```

The first run will also build the containers.
The API should be available at `http://localhost:5000` (check by going to http://localhost:5000/ping - if you see `pong` as a response, you're talking to the service).
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


## Developer Setup

### Running the API locally
You can run the API app on your local machine using `dotnet run` or running directly from Visual Studio. Default hosting URLs are http://localhost:5298 and https://localhost:7298 .

You can interact with the service APIs directly via the Swagger page at https://localhost:7298/swagger/index.html .

There are two database options which can be set up to your development circumstances in the method `ConfigureDb` in the file `BackendServices.cs`.


### Running the UI locally
Run `npm install` to install all dependencies, then run `npm run start` to start the development server.
