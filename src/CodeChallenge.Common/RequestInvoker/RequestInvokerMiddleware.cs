using System;
using System.Net;
using CodeChallenge.Common.Exceptions;
using CodeChallenge.Common.Logging;
using CodeChallenge.Common.RabbitMq;
using Microsoft.AspNetCore.Http;

namespace CodeChallenge.Common.RequestInvoker
{
    public class RequestInvokerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly IRabbitManager _manager;

        public RequestInvokerMiddleware(RequestDelegate next, ILoggerManager logger, IRabbitManager manager)
        {
            _logger = logger;
            _next = next;
            _manager = manager;
        }

        public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                _logger.LogInfo(FormatRequest(httpContext.Request));
                await _next(httpContext);
            }
            catch (CustomException ex)
            {
                _logger.LogError($"Something went wrong. Publishing exception to RabbitMq:\nException source: {ex.Source}\nProblem{ex}");
                
                var errorDetails = new ErrorDetails
                (
                    httpContext.Request.Host + httpContext.Request.Path + httpContext.Request.QueryString,
                    "global-exception-handling",
                    ex.Code,
                    ex.Message,
                    HttpStatusCode.InternalServerError.ToString()
                );
  
                await HandleExceptionAsync(httpContext, errorDetails);
                
                _manager.Publish(errorDetails);
            }
        }

        private static System.Threading.Tasks.Task HandleExceptionAsync(HttpContext context, ErrorDetails errorDetails)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(errorDetails.ToString());
        }

        private string FormatRequest(HttpRequest request)
        {
            return $"[REQUEST INFO] Method: {request.Method} Scheme: {request.Scheme} CotentType: {request.ContentType} Target: {request.Host}{request.Path}{request.QueryString}";
        }
    }
}