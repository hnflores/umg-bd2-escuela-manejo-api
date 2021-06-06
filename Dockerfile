FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API_ESC_MANEJO.API/API_ESC_MANEJO.API.csproj", "API_ESC_MANEJO.API/"]
RUN dotnet restore "API_ESC_MANEJO.API/API_ESC_MANEJO.API.csproj"
COPY . .
WORKDIR "/src/API_ESC_MANEJO.API"
RUN dotnet build "API_ESC_MANEJO.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_ESC_MANEJO.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_ESC_MANEJO.API.dll"]
