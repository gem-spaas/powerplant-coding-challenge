# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PowerplantProductionPlan.sln ./
COPY Directory.Build.props ./
COPY global.json ./
COPY src/Powerplant.Api/Powerplant.Api.csproj src/Powerplant.Api/
COPY tests/Powerplant.Api.Tests/Powerplant.Api.Tests.csproj tests/Powerplant.Api.Tests/

RUN dotnet restore PowerplantProductionPlan.sln

COPY . .
RUN dotnet publish src/Powerplant.Api/Powerplant.Api.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8888 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8888

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Powerplant.Api.dll"]
