# Documents Api WireMock

This docker project creates a mock version of the [Documents Api](https://github.com/nhs-digital-gp-it-futures/BuyingCatalogueDocumentService) for use in integration tests.

It uses the [.Net WireMock Docker Image](https://github.com/WireMock-Net/WireMock.Net-docker/) as a base, and then overrides it to provide default routes and responses as below, which can then be used by the integration tests to validate liveness and provide default valid responses.

- /
- /api/v1/solutions/*/documents

The mappings are set in [default-mapping.json](mappings/default-mapping.json).