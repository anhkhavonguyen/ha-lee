FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.Notification.Api/Harvey.Notification.Api.csproj Harvey.Notification.Api/
COPY Harvey.Notification.Application/Harvey.Notification.Application.csproj Harvey.Notification.Application/
COPY Harvey.Message/Harvey.Message.csproj Harvey.Message/
RUN dotnet restore Harvey.Notification.Api/Harvey.Notification.Api.csproj
COPY . .
WORKDIR /src/Harvey.Notification.Api
RUN dotnet build Harvey.Notification.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Harvey.Notification.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.Notification.Api.dll"]
