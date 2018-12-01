FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.CRMLoyalty.Api/Harvey.CRMLoyalty.Api.csproj Harvey.CRMLoyalty.Api/
COPY Harvey.CRMLoyalty.Application/Harvey.CRMLoyalty.Application.csproj Harvey.CRMLoyalty.Application/
COPY Harvey.Message/Harvey.Message.csproj Harvey.Message/
RUN dotnet restore Harvey.CRMLoyalty.Api/Harvey.CRMLoyalty.Api.csproj
COPY . .
WORKDIR /src/Harvey.CRMLoyalty.Api
RUN dotnet build Harvey.CRMLoyalty.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Harvey.CRMLoyalty.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.CRMLoyalty.Api.dll"]
