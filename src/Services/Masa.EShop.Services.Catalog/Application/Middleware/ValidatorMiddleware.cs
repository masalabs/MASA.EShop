namespace Masa.EShop.Services.Catalog.Application.Middleware;

public class ValidatorMiddleware<TEvent> : Middleware<TEvent>
    where TEvent : notnull, IEvent
{
    private readonly ILogger<ValidatorMiddleware<TEvent>> _logger;
    private readonly IEnumerable<IValidator<TEvent>> _validators;

    public ValidatorMiddleware(IEnumerable<IValidator<TEvent>> validators, ILogger<ValidatorMiddleware<TEvent>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public override async Task HandleAsync(TEvent action, EventHandlerDelegate next)
    {
        var typeName = action.GetType().FullName;

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var failures = _validators
            .Select(v => v.Validate(action))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, action, failures);

            throw new ValidationException("Validation exception", failures);
        }

        await next();
    }
}
