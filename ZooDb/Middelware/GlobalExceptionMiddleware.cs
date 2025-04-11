using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using ZooDb.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  // Continue processing the request
        }
        catch (Exception ex)
        {
            // Catch unhandled exceptions here and log them
            _logger.LogError(ex, "An unhandled exception occurred during request processing.");
            await HandleExceptionAsync(context, ex);  // Handle the exception by sending a response
        }
    }
    
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set default status code and message
        context.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;  // Default to 500 Internal Server Error
        var message = "An unexpected error occurred.";

        if (exception is NotFoundException notFoundException)
    {
        statusCode = HttpStatusCode.NotFound;
        message = string.IsNullOrEmpty(notFoundException.Message) ? "The requested resource could not be found." : notFoundException.Message;
    }
    else if (exception is BadRequestException badRequestException)
    {
        statusCode = HttpStatusCode.BadRequest;
        message = string.IsNullOrEmpty(badRequestException.Message) ? "The request is invalid." : badRequestException.Message;
    }
    else if (exception is UnauthorizedAccessExceptionCustom unauthorizedAccessException)
    {
        statusCode = HttpStatusCode.Unauthorized;
        message = string.IsNullOrEmpty(unauthorizedAccessException.Message) ? "You are not authorized to access this resource." : unauthorizedAccessException.Message;
    }
    else if (exception is InvalidInputException invalidInputException)
    {
        statusCode = HttpStatusCode.BadRequest;
        message = string.IsNullOrEmpty(invalidInputException.Message) ? "The input is invalid." : invalidInputException.Message;
    }
    else if (exception is ArgumentException argumentException)
    {
        statusCode = HttpStatusCode.BadRequest;
        message = string.IsNullOrEmpty(argumentException.Message) ? "An argument error occurred." : argumentException.Message;
    }

        // Other custom exception types can be handled here as needed

        // Return structured error response in JSON format
        var result = new
        {
            error = message,
            statusCode = (int)statusCode
        };

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsJsonAsync(result);  // Send the error response as JSON
    }
}