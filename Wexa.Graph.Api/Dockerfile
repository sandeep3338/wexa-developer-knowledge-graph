FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Wexa.Graph.Api/Wexa.Graph.Api.csproj Wexa.Graph.Api/
RUN dotnet restore Wexa.Graph.Api/Wexa.Graph.Api.csproj

COPY Wexa.Graph.Api/ Wexa.Graph.Api/

WORKDIR /src/Wexa.Graph.Api
RUN dotnet publish Wexa.Graph.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Wexa.Graph.Api.dll"]