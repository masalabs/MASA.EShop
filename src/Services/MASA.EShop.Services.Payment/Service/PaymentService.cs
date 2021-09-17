namespace MASA.EShop.Services.Payment.Service;
public class PaymentService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";
    private readonly IDomainEventBus _eventBus = default!;

    public PaymentService(
        IServiceCollection services,
        IDomainEventBus eventBus)
        : base(services)
    {
        _eventBus = eventBus;

        App.Map("/api/v1/payment/OrderStatusChangedToValidated", HandleAsync);
        // todo remove
        App.Map("/api/v1/payment/HelloWorld", HelloWorld);
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
