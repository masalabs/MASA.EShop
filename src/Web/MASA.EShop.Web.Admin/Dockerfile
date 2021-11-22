#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Web/MASA.EShop.Web.Admin/MASA.EShop.Web.Admin.csproj", "src/Web/MASA.EShop.Web.Admin/"]
RUN dotnet restore "src/Web/MASA.EShop.Web.Admin/MASA.EShop.Web.Admin.csproj"
COPY . .
WORKDIR "/src/src/Web/MASA.EShop.Web.Admin"
RUN dotnet build "MASA.EShop.Web.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.EShop.Web.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.EShop.Web.Admin.dll"]