FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AlleenBijDeVijver.csproj", "./"]
RUN dotnet restore "AlleenBijDeVijver.csproj"
COPY . .
RUN dotnet build "AlleenBijDeVijver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlleenBijDeVijver.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlleenBijDeVijver.dll"]
