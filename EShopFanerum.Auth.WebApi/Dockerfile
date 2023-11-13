FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EShopFanerum.Auth.WebApi/EShopFanerum.Auth.WebApi.csproj", "EShopFanerum.Auth.WebApi/"]
RUN dotnet restore "EShopFanerum.Auth.WebApi/EShopFanerum.Auth.WebApi.csproj"
COPY . .
WORKDIR "/src/EShopFanerum.Auth.WebApi"
RUN dotnet build "EShopFanerum.Auth.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShopFanerum.Auth.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopFanerum.Auth.WebApi.dll"]
