namespace MASA.EShop.Api.Open.Callers.Ordering;

public class OrderingCaller : HttpClientCaller
{
    private readonly ILogger<OrderingCaller> _logger;

    private readonly string _getOrdersUrl = "";
    private readonly string _cancelOrderUrl = "";
    private readonly string _shipOrderUrl = "";
    private string prefix = "/api/v1/orders/";

    public OrderingCaller(
        IServiceProvider serviceProvider,
        IOptions<Settings> settings,
        ILogger<OrderingCaller> logger) : base(serviceProvider)
    {
        BaseAddress = settings.Value.OrderingUrl;
        Name = nameof(OrderingCaller);
        _logger = logger;
        _getOrdersUrl = $"{prefix}list";
        _cancelOrderUrl = $"{prefix}cancel";
        _shipOrderUrl = $"{prefix}ship";
    }

    public async Task<List<OrderSummary>> GetMyOrders(string userId)
    {
        return await CallerProvider.GetFromJsonAsync<List<OrderSummary>>($"{_getOrdersUrl}?userId={userId}") ?? new List<OrderSummary>();
    }

    public async Task<Order> GetOrder(string userId, int orderNumber)
    {
        return await CallerProvider.GetFromJsonAsync<Order>($"{prefix}{userId}/{orderNumber}");
    }

    public async Task<bool> ShipOrder(int orderNumber)
    {
        var stringContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
        });

        var response = await CallerProvider.PutAsync(_shipOrderUrl, stringContent);
        var result = true;
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError($"Ordering Service Request ShipOrder Error:{e}");
            result = false;
        }
        return result;
    }

    public async Task<bool> CancelOrder(int orderNumber)
    {
        var response = await CallerProvider.PutAsync($"{_cancelOrderUrl}/{orderNumber}", null);
        var result = true;
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError($"Ordering Service Request CancelOrder Error:{e}");
            result = false;
        }
        return result;
    }
}

