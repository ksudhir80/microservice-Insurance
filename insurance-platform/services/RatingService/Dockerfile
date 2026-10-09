# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["RatingService.csproj", "./"]

RUN dotnet restore "RatingService.csproj"

COPY . .

RUN dotnet publish "RatingService.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RatingService.dll"]