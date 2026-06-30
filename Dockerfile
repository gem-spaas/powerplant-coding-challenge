FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish PowerPlant.Api -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8888
EXPOSE 8888

ENTRYPOINT ["dotnet", "PowerPlant.Api.dll"]
