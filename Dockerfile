# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["global.json", "./"]
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Presentation/Presentation.csproj", "src/Presentation/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]

RUN dotnet restore "src/WebApi/WebApi.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar y publicar la aplicación
WORKDIR "/src/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Exponer puertos
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copiar archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "WebApi.dll"]
