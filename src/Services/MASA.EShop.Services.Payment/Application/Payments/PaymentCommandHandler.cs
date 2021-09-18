namespace MASA.EShop.Services.Payment.Application.Payments;
public class PaymentCommandHandler
{
    private readonly IPaymentRepository _repository = default!;
    private readonly IOptionsMonitor<AppConfig> _appConfig = default!;
    private readonly PaymentDomainService _paymentDomainService = default!;

    public PaymentCommandHandler(
        IPaymentRepository repository,
        IOptionsMonitor<AppConfig> appConfig,
        PaymentDomainService paymentDomainService)
    {
        _repository = repository;
        _appConfig = appConfig;
        _paymentDomainService = paymentDomainService;
    }

    // todo add dispatch handle attribute
    public async Task HandleAsync(OrderStatusChangedToValidatedCommand command)
    {
        // todo implement log

        //TscLogger.Information(nameof(PaymentCommandHandler), "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", command.Id, _appConfig.CurrentValue.AppId, command);

        await Task.Delay(2000); // Simulation of pay
        var succeeded = Random.Shared.Next(0, 100) >= 50; //50% random success rate

        var payment = new Domain.Payments.Payment(command.OrderId, succeeded);

        await _repository.AddAsync(payment);

        await _paymentDomainService.StatusChangedAsync(payment);
    }
}
