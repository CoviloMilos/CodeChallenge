using Microsoft.AspNetCore.Builder;

namespace CodeChallenge.Common.RequestInvoker
{
    public static class RequestInvokerMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestInvokerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestInvokerMiddleware>();
        }
    }
}