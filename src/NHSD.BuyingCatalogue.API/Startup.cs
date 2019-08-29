using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.API.Extensions;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAllSolutionSummaries;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using System;

namespace NHSD.BuyingCatalogue.API
{
  /// <summary>
  /// Represents a boostrapper for the application. Used as a starting point to configure the API.
  /// </summary>
  public sealed class Startup
  {
    /// <summary>
    /// Application configuration.
    /// </summary>
    private IConfiguration Configuration { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="Startup"/> class.
    /// </summary>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      DumpSettings();
    }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    /// <param name="services">The collection of services.</param>
    /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
    public void ConfigureServices(IServiceCollection services)
    {
      services
        .AddAutoMapper(typeof(AutoMapperProfile).Assembly)
        .AddCustomDbFactory()
        .AddCustomRepositories()
        .AddMediatR(typeof(GetAllSolutionSummariesQueryHandler).Assembly)
              .AddMediatR(typeof(GetSolutionByIdHandler).Assembly)
        .AddCustomSwagger()
        .AddCustomMvc();
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment details.</param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger()
           .UseSwaggerUI(options =>
          {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Buying Catalog API V1");
          });
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      //TODO : Restore HTTPS
      //app.UseHttpsRedirection();
      app.UseMvc();
    }

    private void DumpSettings()
    {
        Console.WriteLine("Settings:");
        Console.WriteLine($"  CONNECTIONSTRINGS:");
        Console.WriteLine($"    BUYINGCATALOGUE: {Configuration.BuyingCatalogueConnectionString()}");
    }
  }
}
