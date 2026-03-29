# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY dotnet/Polyhydra.Ffmpeg.Contracts/Polyhydra.Ffmpeg.Contracts.csproj dotnet/Polyhydra.Ffmpeg.Contracts/
COPY dotnet/Polyhydra.Core.Ffmpeg/Polyhydra.Core.Ffmpeg.csproj dotnet/Polyhydra.Core.Ffmpeg/
COPY dotnet/Polyhydra.Ffmpeg.Host/Polyhydra.Ffmpeg.Host.csproj dotnet/Polyhydra.Ffmpeg.Host/
RUN dotnet restore dotnet/Polyhydra.Ffmpeg.Host/Polyhydra.Ffmpeg.Host.csproj

COPY dotnet/ dotnet/
RUN dotnet publish dotnet/Polyhydra.Ffmpeg.Host/Polyhydra.Ffmpeg.Host.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DISABLE_HTTPS_REDIRECT=true

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Polyhydra.Ffmpeg.Host.dll"]
