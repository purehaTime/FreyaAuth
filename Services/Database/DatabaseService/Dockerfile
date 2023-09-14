FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Database/DatabaseService/DatabaseService.csproj", "Services/Database/DatabaseService/"]
RUN dotnet restore "Services/Database/DatabaseService/DatabaseService.csproj"
COPY . .
WORKDIR "/src/Services/Database/DatabaseService"
RUN dotnet build "DatabaseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DatabaseService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatabaseService.dll"]
