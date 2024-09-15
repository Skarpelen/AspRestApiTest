using System.Text;

namespace AspRestApiTest.Features.Exceptions
{
    using AspRestApiTest.Data;
    using AspRestApiTest.Data.Models;
    using AspRestApiTest.Features.Logger;

    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionHandlingMiddleware(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (ex is SecureException secureException)
                {
                    var queryParameters = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "N/A";
                    var bodyParameters = await GetRequestBodyAsync(context);
                    secureException.QueryParameters = queryParameters;
                    secureException.BodyParameters = bodyParameters;
                    await HandleExceptionAsync(context, secureException);
                }
                else
                {
                    await HandleExceptionAsync(context, ex);
                }
            }
        }

        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    return body;
                }
            }

            return "N/A";
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var eventId = DateTime.UtcNow.Ticks;
            var response = new { id = eventId.ToString() };
            context.Response.ContentType = "application/json";

            if (exception is SecureException secureException)
            {
                await LogExceptionToJournal(eventId, secureException, "Secure");

                context.Response.StatusCode = 500;
                var secureResponse = new
                {
                    type = "Secure",
                    id = eventId,
                    data = new { message = secureException.Message }
                };

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(secureResponse));
            }
            else
            {
                await LogExceptionToJournal(eventId, exception, "Exception");

                context.Response.StatusCode = 500;
                var exceptionResponse = new
                {
                    type = "Exception",
                    id = eventId,
                    data = new { message = $"Internal server error ID = {eventId}" }
                };

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(exceptionResponse));
            }

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }

        private async Task LogExceptionToJournal(long eventId, Exception exception, string type)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var journalEntry = new ExceptionJournal
                {
                    EventId = eventId,
                    Timestamp = DateTime.UtcNow,
                    StackTrace = exception.StackTrace ?? "N/A",
                    ExceptionType = type,
                    QueryParameters = exception is SecureException secureExQ ? secureExQ.QueryParameters : "N/A",
                    BodyParameters = exception is SecureException secureExB ? secureExB.BodyParameters : "N/A"
                };

                try
                {
                    dbContext.ExceptionJournals.Add(journalEntry);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception dbEx)
                {
                    Log.Error($"Failed to log exception to the database. Original exception ID = {eventId}. Error: {dbEx.Message}");
                }
            }
        }
    }
}
