namespace MASA.EShop.Services.Payment.Service;
public class PaymentService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";
    private readonly IEventBus _eventBus = default!;

    public PaymentService(
        WebApplication app,
        IServiceCollection services,
        IEventBus eventBus)
        : base(services)
    {
        _eventBus = eventBus;

        app.Map("/api/v1/payment/OrderStatusChangedToValidated", HandleAsync);
        // todo remove
        app.Map("/api/v1/payment/HelloWorld", HelloWorld);
    }

    public string HelloWorld()
    {
        return "Hello World";
    }

    [Topic(DAPR_PUBSUB_NAME, "OrderStatusChangedToValidatedIntegrationEvent")]
    public async Task HandleAsync(OrderStatusChangedToValidatedCommand cmd)
    {
        await _eventBus.PublishAsync(cmd);
    }
}
