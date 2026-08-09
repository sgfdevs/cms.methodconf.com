FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish
ARG TARGETARCH
WORKDIR /src
COPY src/MethodConf.Cms/MethodConf.Cms.csproj src/MethodConf.Cms/
RUN dotnet restore src/MethodConf.Cms/MethodConf.Cms.csproj --arch $TARGETARCH
COPY src/MethodConf.Cms/ src/MethodConf.Cms/
RUN dotnet publish src/MethodConf.Cms/MethodConf.Cms.csproj -c Release --no-restore --arch $TARGETARCH --self-contained false /p:UseAppHost=false -o /app/publish \
    && mkdir -p \
        /app/publish/wwwroot/media \
        /app/publish/umbraco/Data/TEMP \
        /app/publish/umbraco/Logs \
        /home/app/.aspnet/DataProtection-Keys \
    && touch /app/publish/umbraco/Data/Umbraco.sqlite.db

FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra AS final
ENV ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
COPY --from=publish --chown=$APP_UID:$APP_UID /app/publish .
COPY --from=publish --chown=$APP_UID:$APP_UID /home/app /home/app
LABEL org.opencontainers.image.description="MethodConf Backend - Umbraco CMS"
LABEL org.opencontainers.image.licenses=MIT
EXPOSE 8080
ENTRYPOINT ["dotnet", "MethodConf.Cms.dll"]
