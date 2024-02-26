FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EShopFanerum.StockSevice/EShopFanerum.StockSevice.csproj", "EShopFanerum.StockSevice/"]
RUN dotnet restore "EShopFanerum.StockSevice/EShopFanerum.StockSevice.csproj"
COPY . .
WORKDIR "/src/EShopFanerum.StockSevice"
RUN dotnet build "EShopFanerum.StockSevice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EShopFanerum.StockSevice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopFanerum.StockSevice.dll"]
