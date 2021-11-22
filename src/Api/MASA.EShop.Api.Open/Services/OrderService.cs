namespace MASA.EShop.Api.Open.Services;

public class OrderService : ServiceBase
{
    private readonly OrderingCaller _orderingCaller;

    public OrderService(IServiceCollection services, OrderingCaller orderingCaller) : base(services)
    {
        _orderingCaller = orderingCaller;

        App.MapPut("/api/v1/orders/cancel/{orderNumber:int}", CancelOrderAsync);
        App.MapPut("/api/v1/orders/ship", ShipOrderAsync);
        App.MapGet("/api/v1/orders/{userId}/{orderNumber:int}", GetOrderAsync);
        App.MapGet("/api/v1/orders/list", GetOrdersAsync);
    }

    public async Task<IResult> CancelOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId)
    {
        if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
        {
            return Results.BadRequest();
        }
        if (await _orderingCaller.CancelOrder(orderNumber))
        {
            return Results.Ok();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public async Task<IResult> ShipOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId)
    {
        if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
        {
            return Results.BadRequest();
        }
        if (await _orderingCaller.ShipOrder(orderNumber))
        {
            return Results.Ok();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public async Task<IResult> GetOrderAsync(string userId, int orderNumber)
    {
        var result = await _orderingCaller.GetOrder(userId, orderNumber);
        if (result is null)
        {
            return Results.NotFound();
        }
        else
        {
            return Results.Ok(result);
        }
    }

    public async Task<IResult> GetOrdersAsync(string userId)
    {
        return Results.Ok(await _orderingCaller.GetMyOrders(userId));
    }
}

