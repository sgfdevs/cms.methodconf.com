# MethodConf CMS

The Umbraco CMS and public API for [MethodConf](https://www.methodconf.com/).

## Local Development

1. Copy `.env.example` to `.env` and configure the required values.
2. Start local S3 media storage:

```bash
docker compose up -d seaweedfs
```

3. Run the application:

```bash
dotnet run --project src/MethodConf.Cms/MethodConf.Cms.csproj --launch-profile Umbraco.Web.UI
```

Local media uses SeaweedFS at `http://localhost:8334` with disposable development credentials.

The production image can be built locally with:

```bash
docker build -t cms.methodconf.com .
```
