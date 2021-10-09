namespace MASA.EShop.Services.Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private const string STORE_NAME = "statestore";

    private readonly DaprClient _dapr;

    public BasketRepository(DaprClient dapr)
    {
        _dapr = dapr;
    }

    public async Task DeleteBasketAsync(string id)
    {
        await _dapr.DeleteStateAsync(STORE_NAME, id);
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        return await _dapr.GetStateAsync<CustomerBasket>(STORE_NAME, customerId);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var state = await _dapr.GetStateEntryAsync<CustomerBasket>(STORE_NAME, basket.BuyerId);
        state.Value = basket;
        await state.SaveAsync();

        return await GetBasketAsync(basket.BuyerId);
    }
}
