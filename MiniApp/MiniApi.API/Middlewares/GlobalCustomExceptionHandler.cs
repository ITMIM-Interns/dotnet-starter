using Amazon.S3;
using FluentValidation;
using MiniApp.API.ResponseModel;
using MiniApp.BLL.Exceptions.Commons;
using System.Net;
using System.Net.Mail;

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
        private async Task ExceptionHandleAsync(Exception ex,HttpContext context)
        {

            context.Response.ContentType = "application/json";
            int statusCode = StatusCodes.Status500InternalServerError;
            ExceptionResponse response = new();
            switch (ex)
            {
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    response.Message = ex.Message;
                    break;
                case InvalidAccountException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response.Message = ex.Message;
                    break;
                case ValidationException validation:
                    statusCode = StatusCodes.Status400BadRequest;
                    response.Message = ex.Message;
                    break;
                case SmtpException:
                    statusCode = StatusCodes.Status503ServiceUnavailable;
                    response.Message = ex.Message;
                    break;
                case AmazonS3Exception:
                    statusCode = StatusCodes.Status503ServiceUnavailable;
                    response.Message = ex.Message;
                    break;
                default:
                    response.Message = ex.InnerException?.Message?? ex.Message;
                    break;
            }
            _log.LogError($"{ex.GetType().Name}-->Message:{response.Message}");
            context.Response.StatusCode = statusCode;
            response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
