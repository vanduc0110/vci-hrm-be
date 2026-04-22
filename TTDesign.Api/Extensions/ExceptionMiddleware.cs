using System.Net;
using TTDesign.API.Constants;

namespace TTDesign.API.Extensions
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware( RequestDelegate next, ILogger<ExceptionMiddleware> logger )
    {
      _logger = logger;
      _next = next;
    }
    public async Task InvokeAsync( HttpContext httpContext )
    {
      try {
        await _next( httpContext );
      }
      catch ( Exception ex ) {
        _logger.LogError( $"Something went wrong: {ex}" );
        Console.WriteLine( $"=== EXCEPTION TYPE: {ex.GetType().Name}" );
        Console.WriteLine( $"=== MESSAGE: {ex.Message}" );
        Console.WriteLine( $"=== INNER: {ex.InnerException?.Message}" );
        Console.WriteLine( $"=== INNER2: {ex.InnerException?.InnerException?.Message}" );
        Console.WriteLine( $"=== STACK: {ex.StackTrace}" );
        await HandleExceptionAsync( httpContext, ex );

      }
    }
    private async Task HandleExceptionAsync( HttpContext context, Exception exception )
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = ( int ) HttpStatusCode.InternalServerError;
      await context.Response.WriteAsync( new ErrorDetails()
      {
        StatusCode = context.Response.StatusCode,
        Message = exception.Message + " | " + exception.InnerException?.Message,
      }.ToString() );
    }
  }
}
