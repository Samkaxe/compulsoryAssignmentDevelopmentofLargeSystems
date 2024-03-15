FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AdditionAPI/AdditionAPI.csproj", "AdditionAPI/"]
RUN dotnet restore "AdditionAPI/AdditionAPI.csproj"
COPY . .
WORKDIR "/src/AdditionAPI"
RUN dotnet build "AdditionAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdditionAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdditionAPI.dll"]
