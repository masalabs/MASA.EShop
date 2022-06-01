#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Services/Masa.EShop.Services.Payment/Masa.EShop.Services.Payment.csproj", "src/Services/Masa.EShop.Services.Payment/"]
COPY ["src/Contracts/Masa.EShop.Contracts.Ordering/Masa.EShop.Contracts.Ordering.csproj", "src/Contracts/Masa.EShop.Contracts.Ordering/"]
COPY ["src/Contracts/Masa.EShop.Contracts.Payment/Masa.EShop.Contracts.Payment.csproj", "src/Contracts/Masa.EShop.Contracts.Payment/"]
RUN dotnet restore "src/Services/Masa.EShop.Services.Payment/Masa.EShop.Services.Payment.csproj"
COPY . .
WORKDIR "/src/src/Services/Masa.EShop.Services.Payment"
RUN dotnet build "Masa.EShop.Services.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masa.EShop.Services.Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Masa.EShop.Services.Payment.dll"]