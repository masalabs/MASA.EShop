namespace Masa.EShop.Services.Payment.Application.Payments;

public class PaymentCommandHandler 
{
    private readonly IOptionsMonitor<AppConfig> _appConfig = default!;
    private readonly IPaymentRepository _repository = default!;
    private readonly PaymentDomainService _paymentDomainService = default!;

    public PaymentCommandHandler(
        IOptionsMonitor<AppConfig> appConfig,
        IPaymentRepository repository,
        PaymentDomainService paymentDomainService)
    {
        _repository = repository;
        _appConfig = appConfig;
        _paymentDomainService = paymentDomainService;
    }

    [EventHandler]
    public async Task HandleAsync(OrderStatusChangedToValidatedCommand command)
    {
        //await Task.Delay(2000); // Simulation of pay
        var succeeded = Random.Shared.Next(0, 100) >= 50; //50% random success rate

        var payment = new Domain.Aggregate.Payment(command.OrderId, succeeded);

        await _repository.AddAsync(payment);

        await _paymentDomainService.StatusChangedAsync(payment);
    }
}
