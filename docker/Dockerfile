# Run the API
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY out /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]
