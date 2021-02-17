FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
RUN apk update && apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
COPY out /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "NHSD.BuyingCatalogue.API.dll"]
