FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.Activity.Api/Harvey.Activity.Api.csproj Harvey.Activity.Api/
COPY Harvey.Activity.Application/Harvey.Activity.Application.csproj Harvey.Activity.Application/
COPY Harvey.Message/Harvey.Message.csproj Harvey.Message/
RUN dotnet restore Harvey.Activity.Api/Harvey.Activity.Api.csproj
COPY . .
WORKDIR /src/Harvey.Activity.Api
RUN dotnet build Harvey.Activity.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Harvey.Activity.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.Activity.Api.dll"]
