#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Services/MASA.EShop.Services.Catalog/MASA.EShop.Services.Catalog.csproj", "src/Services/MASA.EShop.Services.Catalog/"]
RUN dotnet restore "src/Services/MASA.EShop.Services.Catalog/MASA.EShop.Services.Catalog.csproj"
COPY . .
WORKDIR "/src/src/Services/MASA.EShop.Services.Catalog"
RUN dotnet build "MASA.EShop.Services.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.EShop.Services.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.EShop.Services.Catalog.dll"]
