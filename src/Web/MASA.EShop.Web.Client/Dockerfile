#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Web/MASA.EShop.Web.Client/MASA.EShop.Web.Client.csproj", "src/Web/MASA.EShop.Web.Client/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Basket/MASA.EShop.Contracts.Basket.csproj", "src/Contracts/MASA.EShop.Contracts.Basket/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Ordering/MASA.EShop.Contracts.Ordering.csproj", "src/Contracts/MASA.EShop.Contracts.Ordering/"]
COPY ["src/Api/MASA.EShop.Api.Caller/MASA.EShop.Api.Caller.csproj", "src/Api/MASA.EShop.Api.Caller/"]
COPY ["src/Contracts/MASA.EShop.Contracts.Catalog/MASA.EShop.Contracts.Catalog.csproj", "src/Contracts/MASA.EShop.Contracts.Catalog/"]
RUN dotnet restore "src/Web/MASA.EShop.Web.Client/MASA.EShop.Web.Client.csproj"
COPY . .
WORKDIR "/src/src/Web/MASA.EShop.Web.Client"
RUN dotnet build "MASA.EShop.Web.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.EShop.Web.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.EShop.Web.Client.dll"]