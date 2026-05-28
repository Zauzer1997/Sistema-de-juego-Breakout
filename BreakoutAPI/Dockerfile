FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todo el código
COPY . .

# Restaura dependencias
RUN dotnet restore "BreakoutAPI.csproj"

# Publica la aplicación
RUN dotnet publish "BreakoutAPI.csproj" -c Release -o /app/publish --no-restore

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BreakoutAPI.dll"]