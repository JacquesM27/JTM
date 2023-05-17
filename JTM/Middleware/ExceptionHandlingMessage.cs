using FluentValidation;
using System.Text.Json;

namespace JTM.Middleware
{
    public class ExceptionHandlingMessage : IMiddleware
    {
        //TODO Add logger
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                Title = GetTitle(exception),
                Status = statusCode,
                Detail = exception.Message,
                Errors = GetErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private int GetStatusCode(Exception exception) =>
            exception switch
            {
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

        private string GetTitle(Exception exception) =>
            exception switch
            {
                ValidationException => "ValidationErrors",
                _ => "ServerError"
            };

        private IEnumerable<object>? GetErrors(Exception exception)
        {
            IEnumerable<object>? errors = exception switch
            {
                ValidationException ex => ex.Errors,
                _ => new List<object> { exception.Message },
            };
            return errors;
        }
    }
}
