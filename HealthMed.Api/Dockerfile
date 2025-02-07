FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

ENV NUGET_PACKAGES=/root/.nuget/fallbackpackages

COPY ../healthmed.sln ./
COPY ../HealthMed.Api/*.csproj HealthMed.Api/
COPY ../HealthMed.Application/*.csproj HealthMed.Application/
COPY ../HealthMed.Domain/*.csproj HealthMed.Domain/
COPY ../HealthMed.Infrastructure/*.csproj HealthMed.Infrastructure/
COPY ../HealthMed.Tests/*.csproj HealthMed.Tests/

RUN dotnet nuget locals all --clear
RUN dotnet restore healthmed.sln

COPY ../ ./
RUN dotnet restore healthmed.sln
RUN dotnet publish HealthMed.Api/HealthMed.Api.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "HealthMed.Api.dll"]
