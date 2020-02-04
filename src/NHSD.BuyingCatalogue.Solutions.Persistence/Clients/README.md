# Document Api Client Generation.

The DocumentsAPI is generated using NSwag, after evaluating AutoRest, NSwag, Swagger Codegen and the OpenAPI Generator. This is because it generates using HttpClient, which allows us to use the new HttpClientFactory in .Net Core 3.0.

In Visual Studio, the generation is done via the extension [APIClientCodeGenerator](https://marketplace.visualstudio.com/items?itemName=ChristianResmaHelle.APIClientCodeGenerator).
