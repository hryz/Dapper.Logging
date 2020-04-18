FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY samples/ApiV3/*.csproj ./samples/ApiV3/
COPY samples/Data/*.csproj ./samples/Data/
COPY src/Dapper.Logging/*.csproj ./src/Dapper.Logging/
RUN dotnet restore

# copy everything else and build app
COPY samples/. ./samples/
COPY src/. ./src/
WORKDIR /app/samples/ApiV3
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/samples/ApiV3/out ./
ENTRYPOINT ["dotnet", "ApiV3.dll"]