# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductionPlanHost/ProductionPlanHost.csproj", "ProductionPlanHost/"]
COPY ["ProductionPlanHost/Application.csproj", "Application/"]
COPY ["ProductionPlanHost/Domain.csproj", "Domain/"]
COPY ["ProductionPlanHost/SharedKernel.csproj", "SharedKernel/"]
RUN dotnet restore "./ProductionPlanHost/ProductionPlanHost.csproj"
COPY . .
WORKDIR "/src/ProductionPlanHost"
RUN dotnet build "./ProductionPlanHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductionPlanHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8888
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductionPlanHost.dll"]