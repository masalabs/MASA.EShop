using MASA.EShop.Contracts.Payment;

namespace MASA.EShop.Services.Ordering.Service
{
    public class OrderEventService : ServiceBase
    {
        private const string DaprPubSubName = "pubsub";

        private readonly ILogger<OrderEventService> _logger;

        public OrderEventService(IServiceCollection services, ILogger<OrderEventService> logger) : base(services)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            App.MapPost("/api/v1/orders/paymentsucceeded", OrderPaymentSucceeded);
            App.MapPost("/api/v1/orders/paymentfailed", OrderPaymentFailed);
            //App.MapPost("/api/v1/orders/StatusChangedToPaid", OrderStatusChangedToPaid);
            //App.MapPost("/api/v1/orders/StatusChangedToShipped", OrderStatusChangedToShipped);
            //App.MapPost("/api/v1/orders/StatusChangedToCancelled", OrderStatusChangedToCancelled);
            App.MapPost("/api/v1/orders/UserCheckoutAccepted", UserCheckoutAccepted);
            App.MapPost("/api/v1/orders/StockConfirmed", OrderStockConfirmed);
            App.MapPost("/api/v1/orders/StockRejected", OrderStockRejected);
        }

        private static IOrderingProcessActor GetOrderingProcessActor(Guid orderId)
        {
            var actorId = new ActorId(orderId.ToString());
            return ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
        }

        [Topic(DaprPubSubName, nameof(UserCheckoutAcceptedIntegrationEvent))]
        public async Task UserCheckoutAccepted(UserCheckoutAcceptedIntegrationEvent integrationEvent)
        {
            if (integrationEvent.RequestId != Guid.Empty)
            {
                var orderingProcess = GetOrderingProcessActor(integrationEvent.RequestId);

                await orderingProcess.Submit(integrationEvent.UserId, integrationEvent.UserName,
                    integrationEvent.Street, integrationEvent.City, integrationEvent.ZipCode,
                    integrationEvent.State, integrationEvent.Country, integrationEvent.Basket);
            }
            else
            {
                _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", integrationEvent);
            }
        }

        [Topic(DaprPubSubName, nameof(OrderPaymentSucceededIntegrationEvent))]
        public Task OrderPaymentSucceeded(OrderPaymentSucceededIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyPaymentSucceeded();
        }

        [Topic(DaprPubSubName, nameof(OrderPaymentFailedIntegrationEvent))]
        public Task OrderPaymentFailed(OrderPaymentFailedIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyPaymentFailed();
        }

        [Topic(DaprPubSubName, nameof(OrderStockConfirmedIntegrationEvent))]
        public Task OrderStockConfirmed(OrderStockConfirmedIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyStockConfirmed();
        }

        [Topic(DaprPubSubName, nameof(OrderStockRejectedIntegrationEvent))]
        public Task OrderStockRejected(OrderStockRejectedIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyStockRejected(
                integrationEvent.OrderStockItems
                    .FindAll(c => !c.HasStock)
                    .Select(c => c.ProductId)
                    .ToList());
        }

        //[HttpPost("OrderStatusChangedToPaid")]
        //[Topic(DaprPubSubName, "OrderStatusChangedToPaidIntegrationEvent")]
        //public Task OrderStatusChangedToPaid(OrderStatusChangedToPaidIntegrationEvent integrationEvent)
        //{
        //    // Save the updated status in the read model and notify the client via SignalR.
        //    //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
        //    //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        //}

        //[HttpPost("OrderStatusChangedToShipped")]
        //[Topic(DaprPubSubName, "OrderStatusChangedToShippedIntegrationEvent")]
        //public Task OrderStatusChangedToShipped(OrderStatusChangedToShippedIntegrationEvent integrationEvent)
        //{
        //    // Save the updated status in the read model and notify the client via SignalR.
        //    //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
        //    //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        //}

        //[HttpPost("OrderStatusChangedToCancelled")]
        //[Topic(DaprPubSubName, "OrderStatusChangedToCancelledIntegrationEvent")]
        //public Task OrderStatusChangedToCancelled(OrderStatusChangedToCancelledIntegrationEvent integrationEvent)
        //{
        //    // Save the updated status in the read model and notify the client via SignalR.
        //    //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
        //    //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        //}
    }
}
