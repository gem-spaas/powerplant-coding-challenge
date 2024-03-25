FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy csproj and restore dependencies for both projects
COPY *.sln .
COPY PowerPlantCC.Api/*.csproj ./PowerPlantCC.Api/
COPY PowerPlantCC.Contracts/*.csproj ./PowerPlantCC.Contracts/
COPY PowerPlantCC.Application/*.csproj ./PowerPlantCC.Application/
COPY PowerPlantCC.Domain/*.csproj ./PowerPlantCC.Domain/
COPY PowerPlantCC.Application.UnitTests/*.csproj ./PowerPlantCC.Application.UnitTests/
COPY PowerPlantCC.Domain.UnitTests/*.csproj ./PowerPlantCC.Domain.UnitTests/
RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
RUN dotnet build -c Release 

# Publish the WebAPI project
WORKDIR /App/PowerPlantCC.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/PowerPlantCC.Api/out .
ENV ASPNETCORE_URLS=http://+:8888
EXPOSE 8888
ENTRYPOINT ["dotnet", "PowerPlantCC.Api.dll"]