# Error Handling Guide

This document describes the global error handling system implemented in the VisitorApp API.

## Overview

The application uses a comprehensive global error handling middleware that:

- Catches all unhandled exceptions across the application
- Logs errors using Serilog with structured logging
- Returns standardized error responses
- Provides different levels of error detail based on environment (Development vs Production)
- Supports multiple exception types with appropriate HTTP status codes

## Error Response Format

All errors return a standardized `ErrorResponse` object:

```json
{
  "message": "Error type description",
  "details": "Specific error details (environment-dependent)",
  "traceId": "unique-trace-identifier",
  "statusCode": 400,
  "timestamp": "2024-01-15T10:30:00Z",
  "errors": {
    "field1": "validation error message",
    "field2": "another validation error"
  }
}
```

## Supported Exception Types

### Domain Exceptions

#### EntityNotFoundException
- **Status Code**: 404 Not Found
- **Usage**: When requested entities don't exist
- **Example**:
```csharp
throw new EntityNotFoundException("Product", productId);
```

#### BusinessLogicException
- **Status Code**: 400 Bad Request
- **Usage**: When business rules are violated
- **Example**:
```csharp
throw new BusinessLogicException("Cannot delete product with active orders");
```

#### DomainException (Base)
- **Status Code**: 400 Bad Request
- **Usage**: General domain rule violations
- **Example**:
```csharp
throw new DomainException("Product name cannot be empty");
```

### System Exceptions

#### ValidationException
- **Status Code**: 400 Bad Request
- **Usage**: Input validation failures
- **Includes**: Validation error details in the `errors` field

#### ArgumentException
- **Status Code**: 400 Bad Request
- **Usage**: Invalid method arguments

#### UnauthorizedAccessException
- **Status Code**: 401 Unauthorized
- **Usage**: Authentication/authorization failures

#### KeyNotFoundException
- **Status Code**: 404 Not Found
- **Usage**: When lookup keys are not found

#### InvalidOperationException
- **Status Code**: 400 Bad Request (in development) / 500 Internal Server Error (in production)
- **Usage**: Invalid operation states

#### All Other Exceptions
- **Status Code**: 500 Internal Server Error
- **Usage**: Unexpected system errors

## Environment-Specific Behavior

### Development Environment
- Full exception messages and stack traces are returned
- Detailed error information for debugging
- Pretty-printed JSON responses

### Production Environment
- Generic error messages for security
- No stack traces or internal details exposed
- Compact JSON responses
- Detailed errors only for domain/business logic exceptions

## Logging

All errors are automatically logged using Serilog with:

- **Exception Details**: Full exception with stack trace
- **Request Information**: Request path and HTTP method
- **User Context**: Current user information (if available)
- **Trace ID**: Unique identifier for request tracking
- **Structured Properties**: For easy querying and filtering

Log entry example:
```
[Error] An error occurred while processing request /api/v1/products/123 with TraceId 12345. User: john.doe
System.InvalidOperationException: Product not found
   at ProductService.GetById(Int32 id)...
```

## Usage Examples

### In Application Services

```csharp
public class ProductService
{
    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null)
        {
            throw new EntityNotFoundException(nameof(Product), id);
        }
        return product;
    }

    public async Task DeleteAsync(int id)
    {
        var hasActiveOrders = await orderRepository.HasActiveOrdersForProduct(id);
        if (hasActiveOrders)
        {
            throw new BusinessLogicException("Cannot delete product with active orders");
        }
        
        await repository.DeleteAsync(id);
    }
}
```

### In API Endpoints

```csharp
public class ProductEndpoint : EndpointBase<GetProductRequest, ProductResponse>
{
    private readonly IProductService productService;

    public override async Task<ProductResponse> ExecuteAsync(GetProductRequest req, CancellationToken ct)
    {
        // Exceptions are automatically handled by the middleware
        var product = await productService.GetByIdAsync(req.Id);
        return mapper.Map<ProductResponse>(product);
    }
}
```

## Testing Error Handling

You can test the error handling middleware using the example endpoint:

```bash
# Test different error types
GET /api/v1/examples/error-handling/validation
GET /api/v1/examples/error-handling/notfound
GET /api/v1/examples/error-handling/business
GET /api/v1/examples/error-handling/domain
GET /api/v1/examples/error-handling/internal
GET /api/v1/examples/error-handling/unauthorized
```

## Configuration

The error handling middleware is automatically registered in the application pipeline via `PipelineConfigurationExtensions.ConfigurePipeline()`. No additional configuration is required.

## Best Practices

1. **Use Specific Exception Types**: Choose the most appropriate exception type for your error scenario
2. **Provide Clear Messages**: Write user-friendly error messages for domain/business exceptions
3. **Don't Catch and Re-throw**: Let the middleware handle exceptions - don't catch them in controllers/endpoints
4. **Log Context**: The middleware automatically logs relevant context, but you can add more using Serilog's `LogContext`
5. **Validate Input**: Use appropriate validation attributes and let ValidationException handle input errors

## Monitoring and Troubleshooting

- **Trace IDs**: Use the `traceId` field to correlate errors across logs and responses
- **Structured Logging**: Query logs using properties like `RequestPath`, `UserName`, etc.
- **Environment Switching**: Test error responses in both Development and Production configurations 