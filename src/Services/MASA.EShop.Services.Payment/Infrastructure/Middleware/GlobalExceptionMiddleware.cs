namespace MASA.EShop.Services.Payment.Infrastructure.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException ex)
        {
            context.Response.StatusCode = 500;
            if (ex.Errors != null && ex.Errors.Any())
            {
                var error = ex.Errors.Select(x => x.ToString()).FirstOrDefault() ?? ex.Message;
                await context.Response.WriteAsync(error);
            }
            else
            {
                await context.Response.WriteAsync(ex.Message);
            }
        }
        catch
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("system error, try again later");
        }
    }
}
