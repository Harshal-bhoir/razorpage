using System.Net;
using System.Text.Json;
using RazorApp.Models;
using Microsoft.ApplicationInsights;
using System;

namespace RazorApp.CustomMiddlewares;

	public class ExceptionHandlingMiddleware
	{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private TelemetryClient _telemetry;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, TelemetryClient telemetryClient)
    {
        _next = next;
        _logger = logger;
        _telemetry = telemetryClient;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _telemetry.TrackException(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal server error "+exception.StackTrace
        };

        
        switch (exception)
        {
            case Microsoft.Azure.Cosmos.CosmosException:
                errorResponse.StatusCode = 400;
                errorResponse.Message = "CosmosDB Exception occured "+exception.Message;
                break;
            case ApplicationException ex:
                if (ex.Message.Contains("Invalid Token"))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.Message = ex.Message;
                    break;
                }
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                break;
            case ArgumentException ex:
                response.StatusCode = 400;
                errorResponse.Message = "Bad/Invalid Argument received";
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Internal server error!";
                break;
        }
        _logger.LogError(exception.Message);
        _telemetry.TrackTrace(exception.StackTrace);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
        throw new Exception("Exception thrown by harshal");
    }
}

