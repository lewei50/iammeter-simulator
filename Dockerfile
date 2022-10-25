# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY HomeSimulator.Web/*.csproj ./HomeSimulator.Web/
RUN dotnet restore

# copy everything else and build app
COPY HomeSimulator.Web/. ./HomeSimulator.Web/
WORKDIR /source/HomeSimulator.Web
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "HomeSimulator.Web.dll"]
