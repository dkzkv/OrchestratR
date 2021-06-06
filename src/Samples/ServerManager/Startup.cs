using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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

namespace ServerManager
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddOrchestratedServerManager()
                .UseSqlServerStorage("Server=localhost,1433;Database=TestDb;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=true")
                .UseRabbitMqTransport(new RabbitMqOptions("localhost", "guest", "guest"));
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