using System;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NHSD.BuyingCatalogue.API.Extensions;
using NHSD.BuyingCatalogue.Application.Infrastructure;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

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
        private IHostingEnvironment CurrentEnvironment { get; }
        private IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup(
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            Configuration = configuration;

            // Environment variable:
            //    ASPNETCORE_ENVIRONMENT == Development
            CurrentEnvironment = env;

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
              .AddMediatR(typeof(ListSolutionsQuery).Assembly)
              .AddCustomSwagger()
              .AddCustomMvc()
              .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
              .AddSingleton<IBearerAuthentication, BearerAuthentication>();

            services
              .AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(options =>
              {
                  var isDev = CurrentEnvironment.IsDevelopment();

                  options.Authority = Configuration.Jwt_Authority();
                  options.Audience = Configuration.Jwt_Audience();
                  options.RequireHttpsMetadata = !CurrentEnvironment.IsDevelopment();
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = async context =>
                      {
                          var auth = ServiceProvider.GetService<IBearerAuthentication>();
                          await auth.OnMessageReceived(context);
                      },

                      OnTokenValidated = async context =>
                      {
                          var auth = ServiceProvider.GetService<IBearerAuthentication>();
                          await auth.OnTokenValidated(context);
                      }
                  };
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Jwt_IssuerSigningKey())),

                      // remove as many restrictions as possible when in dev mode
                      RequireExpirationTime = !isDev,
                      RequireSignedTokens = !isDev,
                      ValidateAudience = !isDev,
                      ValidateIssuer = !isDev,
                      ValidateLifetime = !isDev,
                      ValidateIssuerSigningKey = !isDev
                  };
              });

            if (CurrentEnvironment.IsDevelopment())
            {
                // Register the Swagger generator, defining one or more Swagger documents
                services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = "header",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
            }
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment details.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ServiceProvider = app.ApplicationServices;

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

        /// <summary>
        /// Output all the settings for this application.
        /// </summary>
        private void DumpSettings()
        {
            Console.WriteLine("Settings:");
            Console.WriteLine($"  ConnectionStrings:");
            Console.WriteLine($"    ConnectionStrings:BuyingCatalogue   : {Configuration.BuyingCatalogueConnectionString()}");

            Console.WriteLine($"  Jwt:");
            Console.WriteLine($"    Jwt:UserInfo            : {Configuration.Jwt_UserInfo()}");
            Console.WriteLine($"    Jwt:Authority           : {Configuration.Jwt_Authority()}");
            Console.WriteLine($"    Jwt:Audience            : {Configuration.Jwt_Audience()}");
            Console.WriteLine($"    Jwt:IssuerSigningKey    : {Configuration.Jwt_IssuerSigningKey()}");
        }
    }
}
