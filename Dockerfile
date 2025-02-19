# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /
COPY ["src/Unshackled.Fitness.My/Unshackled.Fitness.My.csproj", "src/Unshackled.Fitness.My/"]
COPY ["src/Unshackled.Fitness.Core/Unshackled.Fitness.Core.csproj", "src/Unshackled.Fitness.Core/"]
COPY ["src/Unshackled.Fitness.Core.Data/Unshackled.Fitness.Core.Data.csproj", "src/Unshackled.Fitness.Core.Data/"]
COPY ["src/Unshackled.Fitness.My.Client/Unshackled.Fitness.My.Client.csproj", "src/Unshackled.Fitness.My.Client/"]
RUN dotnet restore "./src/Unshackled.Fitness.My/Unshackled.Fitness.My.csproj"
COPY . .
COPY ["src/Unshackled.Fitness.My/sample-appsettings.json", "src/Unshackled.Fitness.My/appsettings.json"]
WORKDIR "/src/Unshackled.Fitness.My"
RUN dotnet build "./Unshackled.Fitness.My.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unshackled.Fitness.My.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unshackled.Fitness.My.dll"]