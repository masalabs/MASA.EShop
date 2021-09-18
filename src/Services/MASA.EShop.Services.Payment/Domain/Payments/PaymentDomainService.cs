namespace MASA.EShop.Services.Payment.Domain.Payments;
// todo replace to DomainService implementation
public class PaymentDomainService : IDomainService
{
    private readonly IOptionsMonitor<AppConfig> _appConfig = default!;

    public IDomainEventBus EventBus => throw new NotImplementedException();

    public PaymentDomainService(IOptionsMonitor<AppConfig> appConfig)
    {
        _appConfig = appConfig;
    }

    public async Task StatusChangedAsync(Payment payment)
    {
        IIntegrationDomainEvent orderPaymentIntegrationEvent;

        if (payment.Succeeded)
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededCommand(payment.OrderId);
        }
        else
        {
            //TscLogger.Warning(nameof(PaymentDomainService), "----- Payment rejected for order {OrderId}", command.OrderId);
            orderPaymentIntegrationEvent = new OrderPaymentFailedCommand(payment.OrderId);
        }

        //TscLogger.Information("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentIntegrationEvent.Id, _appConfig.CurrentValue.AppId, orderPaymentIntegrationEvent);

        await EventBus.PublishAsync(orderPaymentIntegrationEvent);
    }
}
