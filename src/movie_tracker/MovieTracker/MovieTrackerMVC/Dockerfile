FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y libfontconfig1
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update && apt-get install -y libfontconfig1
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MovieTrackerMVC/MovieTrackerMVC.csproj", "MovieTrackerMVC/"]
RUN dotnet restore "./MovieTrackerMVC/MovieTrackerMVC.csproj"
COPY . .
WORKDIR "/src/MovieTrackerMVC"
RUN dotnet build "./MovieTrackerMVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieTrackerMVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieTrackerMVC.dll"]
