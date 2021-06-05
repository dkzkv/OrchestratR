using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ServerManager.Extensions
{
    internal static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, string title)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = ApiVersion.Parse("1.0");
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, title));

                var xmlDoc = Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlDoc);
            });
        }
        
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, string title)
        {
            var info = new OpenApiInfo
            {
                Title = title,
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated) info.Description += " This API version has been deprecated.";
            return info;
        }
    }
}