# Run the API
FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
COPY out /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]
