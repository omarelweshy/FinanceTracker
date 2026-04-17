FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY FinanceTracker.Domain/FinanceTracker.Domain.csproj FinanceTracker.Domain/
COPY FinanceTracker.Application/FinanceTracker.Application.csproj FinanceTracker.Application/
COPY FinanceTracker.Infrastructure/FinanceTracker.Infrastructure.csproj FinanceTracker.Infrastructure/
COPY FinanceTracker.Api/FinanceTracker.Api.csproj FinanceTracker.Api/

RUN dotnet restore FinanceTracker.Api/FinanceTracker.Api.csproj

COPY FinanceTracker.Domain/ FinanceTracker.Domain/
COPY FinanceTracker.Application/ FinanceTracker.Application/
COPY FinanceTracker.Infrastructure/ FinanceTracker.Infrastructure/
COPY FinanceTracker.Api/ FinanceTracker.Api/

RUN dotnet publish FinanceTracker.Api/FinanceTracker.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceTracker.Api.dll"]
