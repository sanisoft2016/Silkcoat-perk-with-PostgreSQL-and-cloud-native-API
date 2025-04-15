# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the solution file and restore dependencies
COPY *.sln .
COPY CustRewardMgtSys.API/CustRewardMgtSys.API.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.Application/CustRewardMgtSys.Application.csproj CustRewardMgtSys.Application/
COPY CustRewardMgtSys.Domain/CustRewardMgtSys.Domain.csproj CustRewardMgtSys.Domain/
COPY CustRewardMgtSys.Infrastructure/CustRewardMgtSys.Infrastructure.csproj CustRewardMgtSys.Infrastructure/
RUN dotnet restore

# Copy the entire source code and build the application
COPY . .
RUN dotnet publish CustRewardMgtSys.API/CustRewardMgtSys.API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustRewardMgtSys.API.dll"]
