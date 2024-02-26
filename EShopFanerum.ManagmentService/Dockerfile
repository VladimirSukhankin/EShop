FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EShopFanerum.ManagmentInterface/EShopFanerum.ManagmentInterface.csproj", "EShopFanerum.ManagmentInterface/"]
RUN dotnet restore "EShopFanerum.ManagmentInterface/EShopFanerum.ManagmentInterface.csproj"
COPY . .
WORKDIR "/src/EShopFanerum.ManagmentInterface"
RUN dotnet build "EShopFanerum.ManagmentInterface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShopFanerum.ManagmentInterface.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopFanerum.ManagmentInterface.dll"]
