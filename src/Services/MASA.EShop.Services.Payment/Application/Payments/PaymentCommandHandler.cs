namespace MASA.EShop.Services.Payment.Application.Payments;
public class PaymentCommandHandler
{
    private readonly IPaymentRepository _repository = default!;
    private readonly IDomainEventBus _eventBus = default!;
    private readonly IOptionsMonitor<AppConfig> _appConfig = default!;

    public PaymentCommandHandler(
        IPaymentRepository repository,
        IDomainEventBus eventBus,
        IOptionsMonitor<AppConfig> appConfig)
    {
        _repository = repository;
        _eventBus = eventBus;
        _appConfig = appConfig;
    }

    // todo add dispatch handle attribute
    public async Task HandleAsync(OrderStatusChangedToValidatedCommand command)
    {
        // todo implement log

        //TscLogger.Information(nameof(PaymentCommandHandler), "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", command.Id, _appConfig.CurrentValue.AppId, command);

        IIntegrationDomainEvent orderPaymentIntegrationEvent;

        await Task.Delay(2000); // Simulation of pay
        var succeeded = Random.Shared.Next(0, 100) > 50;

        var payment = new Domain.Entities.Payment(command.OrderId, succeeded);

        await _repository.AddAsync(payment);

        if (payment.Succeeded)
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededCommand(payment.OrderId);
        }
        else
        {
            //TscLogger.Warning(nameof(PaymentCommandHandler), "----- Payment rejected for order {OrderId}", command.OrderId);
            orderPaymentIntegrationEvent = new OrderPaymentFailedCommand(payment.OrderId);
        }

        //TscLogger.Information("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentIntegrationEvent.Id, _appConfig.CurrentValue.AppId, orderPaymentIntegrationEvent);

        await _eventBus.PublishAsync(orderPaymentIntegrationEvent);
    }
}
