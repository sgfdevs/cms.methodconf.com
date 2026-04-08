# MethodConf.com

Simple setup for the MethodConf website.

## Structure

- `frontend/` - Next.js app
- `backend/` - Umbraco/.NET app

## Local Development

1. Copy `frontend/.env.example` to `frontend/.env`.
2. Copy `backend/.env.example` to `backend/.env`.
3. Start the backend:

```bash
dotnet run --project backend/src/MethodConf.Cms/MethodConf.Cms.csproj
```

4. Start the frontend:

```bash
cd frontend && npm install && npm run dev
```
