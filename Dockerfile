FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/MethodConf.Cms/MethodConf.Cms.csproj", "MethodConf.Cms/"]
RUN dotnet restore "MethodConf.Cms/MethodConf.Cms.csproj"
COPY ./src/MethodConf.Cms MethodConf.Cms
WORKDIR "/src/MethodConf.Cms"
RUN dotnet build "MethodConf.Cms.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MethodConf.Cms.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p wwwroot/media && mkdir -p umbraco/Data/TEMP
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MethodConf.Cms.dll"]
