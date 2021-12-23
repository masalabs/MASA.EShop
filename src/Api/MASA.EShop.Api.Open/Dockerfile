#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Api/MASA.EShop.Api.Open/MASA.EShop.Api.Open.csproj", "src/Api/MASA.EShop.Api.Open/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Basket/MASA.EShop.Contracts.Basket.csproj", "src/Contracts/MASA.EShop.Contracts.Basket/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Ordering/MASA.EShop.Contracts.Ordering.csproj", "src/Contracts/MASA.EShop.Contracts.Ordering/"]
COPY ["src/Api/MASA.EShop.Api.Caller/MASA.EShop.Api.Caller.csproj", "src/Api/MASA.EShop.Api.Caller/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Catalog/MASA.EShop.Contracts.Catalog.csproj", "src/Contracts/MASA.EShop.Contracts.Catalog/"]
RUN dotnet restore "src/Api/MASA.EShop.Api.Open/MASA.EShop.Api.Open.csproj"
COPY . .
WORKDIR "/src/src/Api/MASA.EShop.Api.Open"
RUN dotnet build "MASA.EShop.Api.Open.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.EShop.Api.Open.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.EShop.Api.Open.dll"]