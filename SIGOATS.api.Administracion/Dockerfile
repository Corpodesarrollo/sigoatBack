#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG FEED_ACCESSTOKEN
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | sh
# Copia el archivo nuget.config a la ubicación predeterminada de NuGet
COPY ["nuget.config", "/root/.nuget/NuGet/nuget.config"]

COPY ["SIGOATS.api.Administracion/SIGOATS.api.Administracion.csproj", "SIGOATS.api.Administracion/"]
COPY ["SIGOATS.api.Infra/SIGOATS.api.Infra.csproj", "SIGOATS.api.Infra/"]
COPY ["SIGOATS.api.Core/SIGOATS.api.Core.csproj", "SIGOATS.api.Core/"]

ENV FEED_ACCESSTOKEN=${FEED_ACCESSTOKEN}
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://saturno.minsalud.gov.co/tfs/DefaultCollection/_packaging/SISPRO.TRV/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${FEED_ACCESSTOKEN}\"}]}"

RUN dotnet restore "SIGOATS.api.Administracion/SIGOATS.api.Administracion.csproj" --configfile /root/.nuget/NuGet/nuget.config --disable-parallel --ignore-failed-sources --force --no-cache

COPY . .
WORKDIR "/src/SIGOATS.api.Administracion"
RUN dotnet build "SIGOATS.api.Administracion.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SIGOATS.api.Administracion.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIGOATS.api.Administracion.dll"]