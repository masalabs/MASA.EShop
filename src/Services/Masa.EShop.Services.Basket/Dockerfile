#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Services/Masa.EShop.Services.Basket/Masa.EShop.Services.Basket.csproj", "src/Services/Masa.EShop.Services.Basket/"]
COPY ["src/Contracts/Masa.EShop.Contracts.Basket/Masa.EShop.Contracts.Basket.csproj", "src/Contracts/Masa.EShop.Contracts.Basket/"]
COPY ["src/Contracts/Masa.EShop.Contracts.Ordering/Masa.EShop.Contracts.Ordering.csproj", "src/Contracts/Masa.EShop.Contracts.Ordering/"]
RUN dotnet restore "src/Services/Masa.EShop.Services.Basket/Masa.EShop.Services.Basket.csproj"
COPY . .
WORKDIR "/src/src/Services/Masa.EShop.Services.Basket"
RUN dotnet build "Masa.EShop.Services.Basket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masa.EShop.Services.Basket.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Masa.EShop.Services.Basket.dll"]