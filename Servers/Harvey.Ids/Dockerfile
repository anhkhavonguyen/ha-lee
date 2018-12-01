FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.Ids/Harvey.Ids.csproj Harvey.Ids/
COPY Harvey.Message/Harvey.Message.csproj Harvey.Message/
RUN dotnet restore Harvey.Ids/Harvey.Ids.csproj
COPY . .
WORKDIR /src/Harvey.Ids
RUN dotnet build Harvey.Ids.csproj -c Release -o /app


FROM build AS publish
RUN dotnet publish Harvey.Ids.csproj -c Release -o /app


FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.Ids.dll"]
