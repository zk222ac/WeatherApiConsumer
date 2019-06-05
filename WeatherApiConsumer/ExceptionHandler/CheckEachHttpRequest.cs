using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WeatherApiConsumer.ExceptionHandler
{
    public class CheckEachHttpRequest
    {
        private readonly RequestDelegate _next;

        public CheckEachHttpRequest(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected exception occured

            if (exception is HttpRequestException)
            {
                code = HttpStatusCode.BadRequest;
            }
            var result = JsonConvert.SerializeObject(new
            {
                cod = "400",
                error = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }



    }

}
