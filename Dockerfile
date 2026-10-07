# Multi-stage build: the final image has only the runtime and the published app.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/FunBooks.Api/FunBooks.Api.csproj
RUN dotnet publish src/FunBooks.Api/FunBooks.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Run as the built-in non-root user.
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
# The signing key must be supplied at runtime, e.g. -e Jwt__SigningKey=... (from Key Vault in Azure).
ENTRYPOINT ["dotnet", "FunBooks.Api.dll"]
