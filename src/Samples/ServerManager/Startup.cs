using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrchestratR.Extension.RabbitMq;
using OrchestratR.Extension.RabbitMq.Options;
using OrchestratR.ServerManager;
using OrchestratR.ServerManager.Persistence.MsSql;
using Serilog;
using ServerManager.Extensions;
using ServerManager.Extensions.ExceptionsExtension;
using ServerManager.Options;

namespace ServerManager
{
    internal class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration serverManagerOptions)
        {
            _configuration = serverManagerOptions;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var serverManagerOptions = _configuration.GetSection(nameof(ServerManagerOptions))
                .Get<ServerManagerOptions>();
            
            services.AddMvcCore(config =>
                {
                    config.EnableEndpointRouting = false;
                    config.RespectBrowserAcceptHeader = true;
                })
                .AddApiExplorer()
                .AddCors()
                .AddControllersAsServices();

            services.AddControllers();
            services.AddVersionedApiExplorer();
            services.AddSwagger("Orchestrator manager");
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            
            
            //Some new
            
            services.AddOrchestratedServerManager()
                .UseSqlServerStorage(serverManagerOptions.DbConnectionString)
                .UseRabbitMqTransport(new RabbitMqOptions(serverManagerOptions.RabbitMq.Host,
                    serverManagerOptions.RabbitMq.UserName,
                    serverManagerOptions.RabbitMq.Password));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandlerMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });

            app.UseSerilogRequestLogging();
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseMvc();
        }
    }
}