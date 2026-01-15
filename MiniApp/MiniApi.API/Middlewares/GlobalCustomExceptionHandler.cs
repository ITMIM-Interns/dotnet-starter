using FluentValidation;
using Microsoft.Extensions.Logging;
using MiniApp.API.ResponseModel;
using MiniApp.BLL.Exceptions.Commons;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace MiniApp.API.Middlewares
{
    public class GlobalCustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalCustomExceptionHandler> _log;

       

        public GlobalCustomExceptionHandler(RequestDelegate next, ILogger<GlobalCustomExceptionHandler> log)
        {
            _next = next;
            _log = log;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next(context);
            }
            catch (Exception ex)
            {

                await ExceptionHandleAsync(ex, context);
            }
        }
        private async Task ExceptionHandleAsync(Exception exception,HttpContext context)
        {

            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string exType=exception.GetType().ToString();
            ExceptionResponse response = new();
            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response.Message = exception.Message;
                    _log.LogError($"{exType},Message:{response.Message}");
                    break;
                case InvalidAccountException:
                    statusCode = HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    _log.LogError($"{exType},Message:{response.Message}");
                    break;
                case ValidationException validation:
                    statusCode = HttpStatusCode.BadRequest;
                    response.Message = exception.Message;
                    _log.LogError($"{exType},Message:{response.Message}");
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response.Message = exception.Message;
                    _log.LogError($"{exception.GetType},Message:{response.Message}");
                    break;
            }
            context.Response.StatusCode = (int)statusCode;
            response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
