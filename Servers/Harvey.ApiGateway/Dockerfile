FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.ApiGateway/Harvey.ApiGateway.csproj Harvey.ApiGateway/
RUN dotnet restore Harvey.ApiGateway/Harvey.ApiGateway.csproj
COPY . .
WORKDIR /src/Harvey.ApiGateway
RUN dotnet build Harvey.ApiGateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Harvey.ApiGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.ApiGateway.dll"]
