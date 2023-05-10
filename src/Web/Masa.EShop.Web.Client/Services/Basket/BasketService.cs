namespace Masa.EShop.Web.Client.Services.Basket;

public class BasketService : HttpClientCallerBase
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
        BaseAddress = settings.Value.ApiGatewayUrlExternal;
        getBasketUrl = $"{prefix}";
        addItemUrl = $"{prefix}additem";
        removeItemUrl = $"{prefix}removeitem";
        checkoutUrl = $"{prefix}checkout";
    }

    protected override void UseHttpClientPost(MasaHttpClientBuilder masaHttpClientBuilder)
    {
        masaHttpClientBuilder.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        await Caller.PutAsync($"{removeItemUrl}/{userId}/{productId}", null);
    }

    public async Task AddItemToBasketAsync(string userId, int productId)
    {
        await Caller.PutAsync($"{addItemUrl}/{userId}/{productId}", null);
    }

    public async Task<UserBasket?> GetBasketAsync(string userId)
    {
        return await Caller.GetAsync<UserBasket>($"{getBasketUrl}{userId}") ?? new UserBasket(userId, new List<BasketItem>());
    }

    public Task<UserBasket> UpdateBasketAsync(UserBasket basket)
    {
        throw new NotImplementedException();
    }

    public async Task CheckoutAsync(BasketCheckout basketCheckout)
    {
        var response = await Caller.PostAsync(checkoutUrl, basketCheckout);
        response.EnsureSuccessStatusCode();
    }

    protected override string BaseAddress { get; set; }
}

