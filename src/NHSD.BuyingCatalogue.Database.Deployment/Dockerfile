FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dacpacbuild
WORKDIR ~/dacpac
COPY NHSD.BuyingCatalogue.Database.Deployment/*.csproj .
COPY NHSD.BuyingCatalogue.Database .
RUN rm -rf PostDeployment \
    && dotnet build "NHSD.BuyingCatalogue.Database.Deployment.csproj" -c Release -o build

FROM mcr.microsoft.com/mssql-tools:latest AS dacfx
RUN apt-get update && apt-get install libunwind8 libicu-dev wget unzip -y
WORKDIR ~
RUN wget https://go.microsoft.com/fwlink/?linkid=2128144 -O sqlpackage.zip \
    && mkdir sqlpackage \
    && unzip sqlpackage.zip -d /sqlpackage \
    && chmod a+x /sqlpackage/sqlpackage
COPY --from=dacpacbuild ~/dacpac/build/*.dacpac .
COPY NHSD.BuyingCatalogue.Database.Deployment/entrypoint.sh .
COPY NHSD.BuyingCatalogue.Database/PostDeployment .
RUN chmod a+x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
