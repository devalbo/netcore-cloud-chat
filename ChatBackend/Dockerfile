# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ChatBackend/*.csproj ./ChatBackend/
RUN dotnet restore

# copy everything else and build app
COPY ChatBackend/. ./ChatBackend/
WORKDIR /source/ChatBackend
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ChatBackend.dll"]
