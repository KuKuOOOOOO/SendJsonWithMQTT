#Use the official .NET 8 image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MQTTBackend.csproj", "MQTTBackend/"]
RUN dotnet restore "MQTTBackend/MQTTBackend.csproj"

WORKDIR "/src/MQTTBackend"
COPY . .
RUN dotnet build "MQTTBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MQTTBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "MQTTBackend.dll" ]