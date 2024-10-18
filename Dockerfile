
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8888


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Powerplants.Api/Powerplants.Api.csproj", "Powerplants.Api/"]
COPY ["./Powerplants.BusinessLogic/Powerplants.BusinessLogic.csproj", "PowerPlant.BusinessLogic/"]
RUN dotnet restore "Powerplants.Api/Powerplants.Api.csproj"
COPY . .
WORKDIR "/src/Powerplants.Api"
RUN dotnet build "./Powerplants.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
RUN dotnet publish "Powerplants.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Powerplants.Api.dll", "--urls", "http://0.0.0.0:8888"]
