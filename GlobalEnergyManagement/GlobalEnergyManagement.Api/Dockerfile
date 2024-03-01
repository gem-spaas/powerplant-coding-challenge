FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet test

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 8888

ENTRYPOINT ["dotnet", "GlobalEnergyManagement.Api.dll"]