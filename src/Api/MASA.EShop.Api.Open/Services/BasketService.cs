namespace MASA.EShop.Api.Open.Services;

public class BasketService : ServiceBase
{
    private readonly BasketCaller _basketCaller;

    public BasketService(IServiceCollection services, BasketCaller basketCaller) : base(services)
    {
        _basketCaller = basketCaller;

        App.MapGet("/api/v1/basket/{userId}", GetBasketByUserIdAsync);
        App.MapPost("/api/v1/basket/updatebasket", UpdateBasketAsync);
        App.MapPost("/api/v1/basket/checkout", CheckoutAsync);
        App.MapDelete("/api/v1/basket/{userId}", DeleteBasketByIdAsync);
        App.MapPut("/api/v1/basket/additem/{userId}/{productId}", AddItemToBasketAsync);
        App.MapPut("/api/v1/basket/removeitem/{userId}/{productId}", RemoveItemAsync);
    }

    public async Task<IResult> RemoveItemAsync(string userId, int productId)
    {
        await _basketCaller.RemoveItemAsync(userId, productId);
        return Results.Ok();
    }

    public async Task<IResult> AddItemToBasketAsync(string userId, int productId)
    {
        await _basketCaller.AddItemToBasketAsync(userId, productId);
        return Results.Ok();
    }

    public async Task<IResult> GetBasketByUserIdAsync(string userId)
    {
        var basket = await _basketCaller.GetBasketAsync(userId);
        return Results.Ok(basket);
    }

    public async Task<IResult> UpdateBasketAsync([FromBody] UserBasket value)
    {
        return Results.Ok(await _basketCaller.UpdateBasketAsync(value));
    }

    public async Task<IResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout,
        [FromHeader(Name = "x-requestid")] string requestId,
        HttpContext httpContext)
    {
        var userId = basketCheckout.Buyer;

        var basket = await _basketCaller.GetBasketAsync(userId);
        if (basket == null)
        {
            return Results.BadRequest();
        }
        await _basketCaller.CheckoutAsync(basketCheckout);

        return Results.Accepted();
    }

    public async Task DeleteBasketByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}

