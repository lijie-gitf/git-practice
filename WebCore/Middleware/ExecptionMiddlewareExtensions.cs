using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Middleware
{
    public static class ExecptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExecptionMiddleware(this IApplicationBuilder builder)
        {
          return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
