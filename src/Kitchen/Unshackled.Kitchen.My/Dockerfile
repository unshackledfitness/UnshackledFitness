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
COPY ["src/Kitchen/Unshackled.Kitchen.My/Unshackled.Kitchen.My.csproj", "src/Kitchen/Unshackled.Kitchen.My/"]
COPY ["src/Core/Unshackled.Studio.Core.Server/Unshackled.Studio.Core.Server.csproj", "src/Core/Unshackled.Studio.Core.Server/"]
COPY ["src/Core/Unshackled.Studio.Core.Client/Unshackled.Studio.Core.Client.csproj", "src/Core/Unshackled.Studio.Core.Client/"]
COPY ["src/Core/Unshackled.Studio.Core.Data/Unshackled.Studio.Core.Data.csproj", "src/Core/Unshackled.Studio.Core.Data/"]
COPY ["src/Kitchen/Unshackled.Kitchen.Core.Data/Unshackled.Kitchen.Core.Data.csproj", "src/Kitchen/Unshackled.Kitchen.Core.Data/"]
COPY ["src/Kitchen/Unshackled.Kitchen.Core/Unshackled.Kitchen.Core.csproj", "src/Kitchen/Unshackled.Kitchen.Core/"]
COPY ["src/Kitchen/Unshackled.Kitchen.My.Client/Unshackled.Kitchen.My.Client.csproj", "src/Kitchen/Unshackled.Kitchen.My.Client/"]
RUN dotnet restore "./src/Kitchen/Unshackled.Kitchen.My/Unshackled.Kitchen.My.csproj"
COPY . .
COPY ["src/Kitchen/Unshackled.Kitchen.My/sample-appsettings.json", "src/Kitchen/Unshackled.Kitchen.My/appsettings.json"]
WORKDIR "/src/Kitchen/Unshackled.Kitchen.My"
RUN dotnet build "./Unshackled.Kitchen.My.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unshackled.Kitchen.My.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unshackled.Kitchen.My.dll"]