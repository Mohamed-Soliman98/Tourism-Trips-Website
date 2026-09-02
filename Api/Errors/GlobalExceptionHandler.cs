using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Api.Errors
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

           
            var (statusCode, title, detail) = exception switch
            {
                ValidationException or ArgumentException or InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    "Bad Request",
                    exception.Message
                ),
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "Resource Not Found",
                    exception.Message
                ),
                UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message

                ),
                DbUpdateException dbException when dbException.InnerException is SqlException sqlException
                && sqlException.Number == 547 => (
                 StatusCodes.Status400BadRequest,
                 "Bad Request",
                "Cannot delete this resource because it is referenced by other records."
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred. Please try again later."
                )
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; 
        }
    }
}
