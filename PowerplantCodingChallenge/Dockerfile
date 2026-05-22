# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY PowerplantCodingChallenge.slnx ./
COPY PowerplantCodingChallenge.API/PowerplantCodingChallenge.API.csproj ./PowerplantCodingChallenge.API/
COPY PowerplantCodingChallenge.Application/PowerplantCodingChallenge.Application.csproj ./PowerplantCodingChallenge.Application/
COPY PowerplantCodingChallenge.Domain/PowerplantCodingChallenge.Domain.csproj ./PowerplantCodingChallenge.Domain/
COPY PowerplantCodingChallenge.Tests/PowerplantCodingChallenge.Tests.csproj ./PowerplantCodingChallenge.Tests/

# Restore dependencies
RUN dotnet restore PowerplantCodingChallenge.slnx

# Copy all source files
COPY . .

# Build the API project
WORKDIR /src/PowerplantCodingChallenge.API
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose port 8888
EXPOSE 8888

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8888
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "PowerplantCodingChallenge.API.dll"]
