namespace MASA.EShop.Services.Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private const string StoreName = "statestore";

    private readonly DaprClient _dapr;

    public BasketRepository(DaprClient dapr)
    {
        _dapr = dapr;
    }

    public async Task DeleteBasketAsync(string id)
    {
        await _dapr.DeleteStateAsync(StoreName, id);
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        return await _dapr.GetStateAsync<CustomerBasket>(StoreName, customerId);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var state = await _dapr.GetStateEntryAsync<CustomerBasket>(StoreName, basket.BuyerId);
        state.Value = basket;
        await state.SaveAsync();

        return await GetBasketAsync(basket.BuyerId);
    }
}
