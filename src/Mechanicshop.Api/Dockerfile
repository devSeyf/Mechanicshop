# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Mechanicshop.Api/Mechanicshop.Api.csproj", "src/Mechanicshop.Api/"]
COPY ["src/Mechanicshop.Client/Mechanicshop.Client.csproj", "src/Mechanicshop.Client/"]
COPY ["src/Mechanicshop.Contracts/Mechanicshop.Contracts.csproj", "src/Mechanicshop.Contracts/"]
COPY ["src/Mechanicshop.Application/Mechanicshop.Application.csproj", "src/Mechanicshop.Application/"]
COPY ["src/Mechanicshop.Domain/Mechanicshop.Domain.csproj", "src/Mechanicshop.Domain/"]
COPY ["src/Mechanicshop.Infrastructure/Mechanicshop.Infrastructure.csproj", "src/Mechanicshop.Infrastructure/"]
RUN dotnet restore "./src/Mechanicshop.Api/Mechanicshop.Api.csproj"
COPY . .
WORKDIR "/src/src/Mechanicshop.Api"
RUN dotnet build "./Mechanicshop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mechanicshop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mechanicshop.Api.dll"]