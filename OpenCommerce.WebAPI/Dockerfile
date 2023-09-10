FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OpenCommerce.WebAPI/OpenCommerce.WebAPI.csproj", "OpenCommerce.WebAPI/"]
RUN dotnet restore "OpenCommerce.WebAPI/OpenCommerce.WebAPI.csproj"
COPY . .
WORKDIR "/src/OpenCommerce.WebAPI"
RUN dotnet build "OpenCommerce.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCommerce.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenCommerce.WebAPI.dll"]
