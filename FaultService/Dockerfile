FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FaultService/FaultService.csproj", "FaultService/"]
RUN dotnet restore "FaultService/FaultService.csproj"
COPY . .
WORKDIR "/src/FaultService"
RUN dotnet build "FaultService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FaultService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FaultService.dll"]
