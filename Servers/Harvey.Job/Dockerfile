FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Harvey.Job/Harvey.Job.csproj Harvey.Job/
COPY Harvey.Message/Harvey.Message.csproj Harvey.Message/
RUN dotnet restore Harvey.Job/Harvey.Job.csproj
COPY . .
WORKDIR /src/Harvey.Job
RUN dotnet build Harvey.Job.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Harvey.Job.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Harvey.Job.dll"]
