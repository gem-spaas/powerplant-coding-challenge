FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish PowerPlant.Api -c Release -o PowerPlant.Api/out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/PowerPlant.Api/out .


ENTRYPOINT ["dotnet", "PowerPlant.Api.dll"]