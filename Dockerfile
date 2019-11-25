FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /app

# Copy application projects
COPY *.sln .
COPY src/NHSD.BuyingCatalogue.Contracts/*.csproj ./src/NHSD.BuyingCatalogue.Contracts/
COPY src/NHSD.BuyingCatalogue.API/*.csproj ./src/NHSD.BuyingCatalogue.API/
COPY src/NHSD.BuyingCatalogue.Capabilities.API/*.csproj ./src/NHSD.BuyingCatalogue.Capabilities.API/
COPY src/NHSD.BuyingCatalogue.Capabilities.Application/*.csproj ./src/NHSD.BuyingCatalogue.Capabilities.Application/
COPY src/NHSD.BuyingCatalogue.SolutionList.API/*.csproj ./src/NHSD.BuyingCatalogue.SolutionList.API/
COPY src/NHSD.BuyingCatalogue.Solution.API/*.csproj ./src/NHSD.BuyingCatalogue.Solution.API/
COPY src/NHSD.BuyingCatalogue.Application/*.csproj ./src/NHSD.BuyingCatalogue.Application/
COPY src/NHSD.BuyingCatalogue.Infrastructure/*.csproj ./src/NHSD.BuyingCatalogue.Infrastructure/
COPY src/NHSD.BuyingCatalogue.Persistence/*.csproj ./src/NHSD.BuyingCatalogue.Persistence/
COPY src/NHSD.BuyingCatalogue.Data/*.csproj ./src/NHSD.BuyingCatalogue.Data/

# Copy test projects
COPY tests/NHSD.BuyingCatalogue.API.IntegrationTests/*.csproj ./tests/NHSD.BuyingCatalogue.API.IntegrationTests/
COPY tests/NHSD.BuyingCatalogue.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Capabilities.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Capabilities.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Capabilities.Application.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Capabilities.Application.UnitTests/
COPY tests/NHSD.BuyingCatalogue.SolutionList.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.SolutionList.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Solution.API.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Solution.API.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Application.UnitTests/*.csproj ./tests/NHSD.BuyingCatalogue.Application.UnitTests/
COPY tests/NHSD.BuyingCatalogue.Data.Tests/*.csproj ./tests/NHSD.BuyingCatalogue.Data.Tests/
COPY tests/NHSD.BuyingCatalogue.Persistence.DatabaseTests/*.csproj ./tests/NHSD.BuyingCatalogue.Persistence.DatabaseTests/
COPY tests/NHSD.BuyingCatalogue.Infrastructure.Tests/*.csproj ./tests/NHSD.BuyingCatalogue.Infrastructure.Tests/
COPY tests/NHSD.BuyingCatalogue.Testing.Data/*.csproj ./tests/NHSD.BuyingCatalogue.Testing.Data/
COPY tests/NHSD.BuyingCatalogue.Testing.Tools/*.csproj ./tests/NHSD.BuyingCatalogue.Testing.Tools/

# Restore main application and the test unit test project
RUN dotnet restore

# Copy full solution over
COPY . .
RUN dotnet build

FROM build AS testrunner
CMD ["dotnet", "test", "--logger:trx"]

# run the unit tests
FROM build AS test
RUN dotnet test --logger:trx

# Publish the API
FROM build AS publish
WORKDIR /app/src/NHSD.BuyingCatalogue.API
RUN dotnet publish -c Release -o out

# Run the API
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app
COPY --from=publish /app/src/NHSD.BuyingCatalogue.API/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]
