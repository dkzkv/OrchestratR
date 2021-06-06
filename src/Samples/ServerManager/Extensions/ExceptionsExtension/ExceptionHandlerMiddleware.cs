using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrchestratR.ServerManager.Domain.Common.Exceptions;
using ServerManager.v1.Models;

namespace ServerManager.Extensions.ExceptionsExtension
{
    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ExistedJobException existedJobException)
            {
                await HandleValidExceptionMessageAsync(context,HttpStatusCode.Conflict ,new Problem()
                {
                    Code = "ExistedJob",
                    Title = "Job already exist",
                    Details = existedJobException.Message
                });
            }
            catch (NotExistedJobException notExistedJobException)
            {
                await HandleValidExceptionMessageAsync(context, HttpStatusCode.Conflict ,new Problem()
                {
                    Code = "NotExistedJob",
                    Title = "Job not exist",
                    Details = notExistedJobException.Message
                });
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandleValidExceptionMessageAsync(HttpContext context, HttpStatusCode statusCode, Problem problem)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                Code = problem.Code,
                Title = problem.Title,
                Details = problem.Details
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int) HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}