using FluentValidation;
using JTM.DTO.ExceptionResponse;
using JTM.Exceptions;
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new ExceptionResponse(
                title: GetTitle(exception),
                statusCode: statusCode,
                errors: GetErrors(exception));

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private int GetStatusCode(Exception exception) 
            => exception switch
            {
                ValidationException => StatusCodes.Status403Forbidden,
                AuthException => StatusCodes.Status400BadRequest,
                CompanyException => StatusCodes.Status400BadRequest,
                WorkingTimeException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

        private string GetTitle(Exception exception)
            => exception switch
            {
                ValidationException => "ValidationErrors",
                AuthException => "UserErrors",
                CompanyException => "CompanyException",
                WorkingTimeException => "WorkingTimeException",
                _ => "ServerError"
            };

        private IEnumerable<ExceptionError> GetErrors(Exception exception)
        {
            IEnumerable<ExceptionError> errors = exception switch
            {
                ValidationException ex => ex.Errors.Select(err => new ExceptionError(err.PropertyName, err.ErrorMessage)),
                _ => new List<ExceptionError> { new ExceptionError(exception.Message) }
            };
            return errors;
        }
    }
}
