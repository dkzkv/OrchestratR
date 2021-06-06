using Microsoft.AspNetCore.Builder;

namespace ServerManager.Extensions.ExceptionsExtension
{
    internal static class ExceptionHandlerMiddlewareExtensions  
    {  
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)  
        {  
            app.UseMiddleware<ExceptionHandlerMiddleware>();  
        }  
    }
}