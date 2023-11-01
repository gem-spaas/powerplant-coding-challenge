FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["PowerPlant.Api/PowerPlant.Api.csproj", "PowerPlant.Api/"]   
COPY ["PowerPlant.Domain/PowerPlant.Domain.csproj", "PowerPlant.Domain/"]
COPY ["PowerPlant.Services/PowerPlant.Services.csproj", "PowerPlant.Services/"]
RUN dotnet restore "PowerPlant.Api/PowerPlant.Api.csproj"
COPY . .
WORKDIR "/src/PowerPlant.Api"
RUN dotnet build "PowerPlant.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerPlant.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PowerPlant.Api.dll"]