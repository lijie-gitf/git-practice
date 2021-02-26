using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Middleware
{
    /// <summary>
    /// 处理异常中间件
    /// </summary>
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        private IWebHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try 
            {
                await _next.Invoke(context);
                
            }
            catch (Exception ex)
            {
               await HandleException(context,ex);
            }
        
        }

      public async Task  HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string error = string.Empty;
            if (_environment.IsDevelopment() || 1==1)
            {
                var json = new { message = e.Message };
                error = JsonConvert.SerializeObject(json);
            }
            else {
                error = "出错了";
            }
            await context.Response.WriteAsync(error);
        }
    }
}
