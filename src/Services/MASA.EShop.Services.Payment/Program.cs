var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLazyWebApplication(builder)
    // todo refactor
    .AddScoped<IDomainEventBus, DomainEventBus>()
    .AddServices();

var app = builder.Services.BuildServiceProvider().GetRequiredService<WebApplication>();

app.Run();

// todo remove
public class DomainEventBus : IDomainEventBus
{
    public Task PublishAsync<TDomentEvent>(TDomentEvent @event) where TDomentEvent : IDomainEvent
    {
        throw new NotImplementedException();
    }
}
