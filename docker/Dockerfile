# Run the API
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-bionic AS runtime
COPY out /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]
