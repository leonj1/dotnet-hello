#!/bin/sh
set -e

# Create test output directories
mkdir -p /app/TestResults /app/coverage
chmod -R 777 /app/TestResults /app/coverage

# Restore dependencies
dotnet restore

# Run tests with coverage
dotnet test \
    --logger "console;verbosity=detailed" \
    --no-restore \
    --blame-hang-timeout 60s \
    --diag /app/TestResults/diagnostics.log \
    /p:CollectCoverage=true \
    /p:CoverletOutput=/app/coverage/ \
    /p:CoverletOutputFormat=cobertura \
    /p:Exclude="[xunit.*]*" \
    /app/WeatherApi.Tests/WeatherApi.Tests.csproj

# Generate coverage report
reportgenerator \
    -reports:"/app/coverage/coverage.cobertura.xml" \
    -targetdir:"/app/coverage" \
    -reporttypes:Html
