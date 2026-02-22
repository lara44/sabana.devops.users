# Sabana DevOps Users API

## 🐳 Docker

### Construir la imagen

```bash
docker build -t sabana-devops-users:latest .
```

### Ejecutar el contenedor

```bash
docker run -d \
  --name users-api \
  -p 5122:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  sabana-devops-users:latest
```

### Acceder a la aplicación

- API: http://localhost:5122/api/users
- Documentación Scalar: http://localhost:5122/scalar/v1

### Detener y eliminar el contenedor

```bash
docker stop users-api
docker rm users-api
```

## 🐳 Docker Compose (Alternativa)

### Construir y levantar

```bash
docker compose up -d
```

### Detener

```bash
docker compose down
```
