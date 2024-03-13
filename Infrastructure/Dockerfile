FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Infrastructure/Infrastructure.csproj"
COPY . .
WORKDIR "/src/Infrastructure"
RUN dotnet build "Infrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Infrastructure.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Infrastructure.dll"]
