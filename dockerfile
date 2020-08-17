FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and call `restore` as a distinct layer
COPY *.sln .
COPY samples/ApiV3/*.csproj ./samples/ApiV3/
COPY samples/Data/*.csproj ./samples/Data/
COPY src/Dapper.Logging/*.csproj ./src/Dapper.Logging/
COPY tests/Dapper.Logging.Tests/*.csproj ./tests/Dapper.Logging.Tests/
RUN dotnet restore --verbosity normal

# copy the actual sources and build app
COPY samples/. ./samples/
COPY src/. ./src/
COPY strong-name.snk .
WORKDIR /app/samples/ApiV3
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/samples/ApiV3/out ./
ENTRYPOINT ["dotnet", "ApiV3.dll"]