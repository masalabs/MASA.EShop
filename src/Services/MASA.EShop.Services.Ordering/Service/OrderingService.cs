namespace MASA.EShop.Services.Ordering.Service
{
    public class OrderingService : ServiceBase
    {
        public OrderingService(IServiceCollection services) : base(services)
        {
            App.MapPut("/api/v1/orders/cancel/{orderNumber:int}", CancelOrderAsync);
            App.MapPut("/api/v1/orders/ship", ShipOrderAsync);
            App.MapGet("/api/v1/orders/{orderNumber:int}", GetOrderAsync);
            App.MapGet("/api/v1/orders/list", GetOrdersAsync);
            App.MapGet("/api/v1/orders/cardtypes", GetCardTypesAsync);
        }

        public async Task<IResult> CancelOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId, [FromServices] IEventBus eventBus)
        {
            try
            {
                if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
                {
                    throw new Exception("invalid requestid");
                }
                var orderCanelCommand = new OrderCancelCommand
                {
                    OrderNumber = orderNumber
                };
                await eventBus.PublishAsync(orderCanelCommand);
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }

        public async Task<IResult> ShipOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId, [FromServices] IEventBus eventBus)
        {
            try
            {
                if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
                {
                    throw new Exception("invalid requestid");
                }
                var orderShipCommand = new OrderShipCommand
                {
                    OrderNumber = orderNumber
                };
                await eventBus.PublishAsync(orderShipCommand);
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }

        public async Task<IResult> GetOrderAsync(string userId, int orderNumber, [FromServices] IEventBus eventBus)
        {
            var orderQuery = new OrderQuery
            {
                OrderNumber = orderNumber,
                UserId = userId
            };
            await eventBus.PublishAsync(orderQuery);
            if (orderQuery.Result is null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(OrderDto.FromOrder(orderQuery.Result));
            }
        }

        public async Task<IResult> GetOrdersAsync([FromServices] IEventBus eventBus, string userId)
        {
            var ordersQuery = new OrdersQuery
            {
                BuyerId = userId
            };
            await eventBus.PublishAsync(ordersQuery);
            return Results.Ok(ordersQuery.Result.Select(OrderSummaryDto.FromOrderSummary));
        }

        public async Task<IResult> GetCardTypesAsync([FromServices] IEventBus eventBus)
        {
            var cardTypesQuery = new CardTypesQuery();
            await eventBus.PublishAsync(cardTypesQuery);
            return Results.Ok(cardTypesQuery.Result.Select(CardTypeDto.FromCardType));
        }
    }
}
