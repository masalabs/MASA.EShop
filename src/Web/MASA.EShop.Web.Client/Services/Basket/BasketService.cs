namespace MASA.EShop.Web.Client.Services.Basket;

public class BasketService : HttpClientCaller
{
    private readonly IServiceProvider _serviceProvider;
    private string getBasketUrl;
    private readonly string addItemUrl;
    private readonly string checkoutUrl;
    private readonly string removeItemUrl;
    private readonly string prefix = "/api/v1/basket/";

    public BasketService(IServiceProvider serviceProvider, IOptions<Settings> settings) : base(serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Name = nameof(BasketService);
        BaseAddress = settings.Value.ApiGatewayUrlExternal;
        getBasketUrl = $"{prefix}";
        addItemUrl = $"{prefix}additem";
        removeItemUrl = $"{prefix}removeitem";
        checkoutUrl = $"{prefix}checkout";
    }

    public override DelegatingHandler CreateHttpMessageHandler()
    {
        return _serviceProvider.GetRequiredService<HttpClientAuthorizationDelegatingHandler>();
    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        await CallerProvider.PutAsync($"{removeItemUrl}/{userId}/{productId}", null);
    }

    public async Task AddItemToBasketAsync(string userId, int productId)
    {
        await CallerProvider.PutAsync($"{addItemUrl}/{userId}/{productId}", null);
    }

    public async Task<UserBasket?> GetBasketAsync(string userId)
    {
        return await CallerProvider.GetFromJsonAsync<UserBasket>($"{getBasketUrl}{userId}") ?? new UserBasket(userId, new List<BasketItem>());
    }

    public Task<UserBasket> UpdateBasketAsync(UserBasket basket)
    {
        throw new NotImplementedException();
    }

    public async Task CheckoutAsync(BasketCheckout basketCheckout)
    {
        var response = await CallerProvider.PostAsJsonAsync(checkoutUrl, basketCheckout);
        response.EnsureSuccessStatusCode();
    }
}

