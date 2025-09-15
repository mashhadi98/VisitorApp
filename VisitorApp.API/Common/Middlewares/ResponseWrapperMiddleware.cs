using System.Text.Json;
using VisitorApp.API.Common.Models;

namespace VisitorApp.API.Common.Middlewares;

/// <summary>
/// Middleware to wrap successful responses in a standard format
/// </summary>
public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ResponseWrapperMiddleware> logger;

    public ResponseWrapperMiddleware(RequestDelegate next, ILogger<ResponseWrapperMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip wrapping for certain endpoints
        if (ShouldSkipWrapping(context))
        {
            await next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;
        
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        // Only wrap successful responses (2xx status codes)
        if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
        {
            await WrapSuccessResponse(context, responseBody, originalBodyStream);
        }
        else
        {
            // For error responses, just copy the response as-is (handled by GlobalErrorHandlingMiddleware)
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task WrapSuccessResponse(HttpContext context, MemoryStream responseBody, Stream originalBodyStream)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

        object data;
        
        // Try to parse existing JSON response, fallback to raw string if not JSON
        try
        {
            if (!string.IsNullOrEmpty(responseContent))
            {
                data = JsonSerializer.Deserialize<object>(responseContent) ?? new { };
            }
            else
            {
                data = new { };
            }
        }
        catch
        {
            // If not valid JSON, wrap the raw content
            data = string.IsNullOrEmpty(responseContent) ? new { } : responseContent;
        }

        var standardResponse = new StandardResponse();
        standardResponse = standardResponse.StandardSuccessResponse
        (
            data: data,
            statusCode: context.Response.StatusCode,
            traceId: context.TraceIdentifier
        );

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var wrappedResponse = JsonSerializer.Serialize(standardResponse, jsonOptions);
        
        // Update content type and length
        context.Response.ContentType = "application/json";
        context.Response.ContentLength = null; // Let the framework calculate this
        
        // Write the wrapped response
        context.Response.Body = originalBodyStream;
        await context.Response.WriteAsync(wrappedResponse);
    }

    private static bool ShouldSkipWrapping(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();
        
        // Skip wrapping for swagger, health checks, static files, etc.
        return path?.Contains("/swagger") == true ||
               path?.Contains("/health") == true ||
               path?.Contains("/_framework") == true ||
               path?.Contains("/css") == true ||
               path?.Contains("/js") == true ||
               path?.Contains("/images") == true ||
               context.Response.ContentType?.Contains("text/html") == true;
    }
} 