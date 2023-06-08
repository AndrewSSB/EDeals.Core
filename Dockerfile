#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EDeals.Core/EDeals.Core.csproj", "src/EDeals.Core/"]
COPY ["src/EDeals.Core.Infrastructure/EDeals.Core.Infrastructure.csproj", "src/EDeals.Core.Infrastructure/"]
COPY ["src/EDeals.Core.Application/EDeals.Core.Application.csproj", "src/EDeals.Core.Application/"]
COPY ["src/EDeals.Core.Domain/EDeals.Core.Domain.csproj", "src/EDeals.Core.Domain/"]
RUN dotnet restore "src/EDeals.Core/EDeals.Core.csproj"
COPY . .
WORKDIR "/src/src/EDeals.Core"
RUN dotnet build "EDeals.Core.csproj" -c Release --no-restore

RUN dotnet publish "EDeals.Core.csproj" -c Release --no-build --output /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "EDeals.Core.dll"]