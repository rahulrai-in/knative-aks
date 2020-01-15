FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Hello-Orchestrator.csproj", "Hello-Orchestrator/"]
RUN dotnet restore "Hello-Orchestrator/Hello-Orchestrator.csproj"
COPY . "Hello-Orchestrator/"
WORKDIR "/src/Hello-Orchestrator"
RUN dotnet build "Hello-Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hello-Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hello-Orchestrator.dll"]