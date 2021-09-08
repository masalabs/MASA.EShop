var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

builder.Services
    // todo refactor to MinimalAPIs
    .AddSingleton(app)
    // todo refactor to MinimalAPIs
    .AddScoped<IServiceCollection>(sp => builder.Services)
    // todo refactor
    .AddScoped<IDomainEventBus, DomainEventBus>()
    .AddServices();

app.MapGet("/", () => "Hello World!");

app.Run();

// todo remove
public class DomainEventBus : IDomainEventBus
{
    public Task PublishAsync<TDomentEvent>(TDomentEvent @event) where TDomentEvent : IDomainEvent
    {
        throw new NotImplementedException();
    }
}
