#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Services/MASA.EShop.Services.Payment/MASA.EShop.Services.Payment.csproj", "src/Services/MASA.EShop.Services.Payment/"]
RUN dotnet restore "src/Services/MASA.EShop.Services.Payment/MASA.EShop.Services.Payment.csproj"
COPY . .
WORKDIR "/src/src/Services/MASA.EShop.Services.Payment"
RUN dotnet build "MASA.EShop.Services.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.EShop.Services.Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.EShop.Services.Payment.dll"]
