namespace MASA.EShop.Services.Basket.Infrastructure.Events;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(OrderStartedIntegrationEvent);
}