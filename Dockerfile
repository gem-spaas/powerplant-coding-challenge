 FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=RELEASE
WORKDIR /src

COPY ["GEM.PowerPlant.Api/GEM.PowerPlant.Api.csproj", "GEM.PowerPlant.Api/"]
COPY ["GEM.PowerPlant.Domain/GEM.PowerPlant.Domain.csproj", "GEM.PowerPlant.Domain/"]
COPY ["GEM.PowerPlant.Application/GEM.PowerPlant.Application.csproj", "GEM.PowerPlant.Application/"]
COPY ["GEM.PowerPlant.Api.Test/GEM.PowerPlant.Api.Test.csproj", "GEM.PowerPlant.Api.Test/"]
RUN dotnet restore "GEM.PowerPlant.Api/GEM.PowerPlant.Api.csproj"

COPY . .
WORKDIR "/src/GEM.PowerPlant.Api/"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build 

FROM build AS publish
ARG BUILD_CONFIGURATION=RELEASE
WORKDIR /src/GEM.PowerPlant.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish 

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish app/publish .
ENTRYPOINT ["dotnet", "GEM.PowerPlant.Api.dll"]