#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HomeSimulator.Web/*.csproj", "HomeSimulator.Web/"]
RUN dotnet restore "HomeSimulator.Web/HomeSimulator.Web.csproj"
COPY . .
WORKDIR "/src/HomeSimulator.Web"
RUN dotnet build "HomeSimulator.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeSimulator.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeSimulator.Web.dll"]