# MethodConf CMS

The Umbraco CMS and public API for [MethodConf](https://www.methodconf.com/).

## Local Development

1. Copy `.env.example` to `.env` and configure the required values.
2. Run the application:

```bash
dotnet run --project src/MethodConf.Cms/MethodConf.Cms.csproj
```

The production image can be built locally with:

```bash
docker build -t cms.methodconf.com .
```
