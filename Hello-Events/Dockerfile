FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Hello-Events.csproj", "Hello-Events/"]
RUN dotnet restore "Hello-Events/Hello-Events.csproj"
COPY . "Hello-Events/"
WORKDIR "/src/Hello-Events"
RUN dotnet build "Hello-Events.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hello-Events.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hello-Events.dll"]