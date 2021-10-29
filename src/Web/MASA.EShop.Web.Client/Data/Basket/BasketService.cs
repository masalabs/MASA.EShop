namespace MASA.EShop.Web.Client.Data.Basket;

public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BasketService> _logger;
    private readonly ICatalogService _catalogService;

    private string getBasketUrl;
    private readonly string updateBasketUrl;
    private readonly string checkoutUrl;

    public BasketService(HttpClient httpClient, IOptions<Settings> settings, ILogger<BasketService> logger, ICatalogService catalogService)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BasketUrl);
        _logger = logger;
        _catalogService = catalogService;

        var party = "/api/v1/basket/";

        getBasketUrl = $"{party}";
        updateBasketUrl = $"{party}updatebasket";
        checkoutUrl = $"{party}checkout";

    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        var currentBasket = await GetBasketAsync(userId);
        var basketItem = currentBasket.Items.FirstOrDefault(x => x.ProductId == productId);
        if (basketItem != null)
        {
            currentBasket.Items.Remove(basketItem);
            var response = await _httpClient.PostAsJsonAsync(updateBasketUrl, currentBasket);
        }
    }

    public async Task AddItemToBasketAsync(string userId, int productId)
    {
        var newItem = new
        {
            CatalogItemId = productId,
            BasketId = userId,
            Quantity = 1
        };

        //todo move to api open (bfff)

        // Step 1: Get the item from catalog
        var item = await _catalogService.GetCatalogById(newItem.CatalogItemId);
        // Step 2: Get current basket status
        var currentBasket = await GetBasketAsync(userId);
        // Step 3: Merge current status with new product
        var itemIndex = currentBasket.Items.FindIndex(a => a.ProductId == item.Id);
        if (itemIndex == -1)
        {
            currentBasket.Items.Add(new BasketItem(item.Id, item.Name, item.Price, newItem.Quantity, item.PictureFileName));
        }
        else
        {
            var basketItem = currentBasket.Items.ElementAt(itemIndex);
            currentBasket.Items[itemIndex] = basketItem with { Quantity = basketItem.Quantity + 1 };
        }

        // Step 4: Update basket
        var response = await _httpClient.PostAsJsonAsync(updateBasketUrl, currentBasket);
        //await _basket.UpdateAsync(currentBasket);
    }

    public async Task<UserBasket> GetBasketAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<UserBasket>($"{getBasketUrl}{userId}");
    }

    public Task<UserBasket> UpdateBasketAsync(UserBasket basket)
    {
        throw new NotImplementedException();
    }

    public async Task CheckoutAsync(BasketCheckout basketCheckout)
    {
        var response = await _httpClient.PostAsJsonAsync(checkoutUrl, basketCheckout);

        response.EnsureSuccessStatusCode();
    }
}
