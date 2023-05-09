namespace Masa.EShop.Api.Open.Callers.Ordering;

public class OrderingCaller : HttpClientCallerBase
{
    protected override string Prefix { get; set; } = "/api/v1/orders/";

    private readonly ILogger<OrderingCaller> _logger;

    private readonly string _getOrdersUrl;
    private readonly string _cancelOrderUrl;
    private readonly string _shipOrderUrl;

    public OrderingCaller(
        IServiceProvider serviceProvider,
        IOptions<Settings> settings,
        ILogger<OrderingCaller> logger) : base(serviceProvider)
    {
        BaseAddress = settings.Value.OrderingUrl;
        _logger = logger;
        _getOrdersUrl = $"list";
        _cancelOrderUrl = $"cancel";
        _shipOrderUrl = $"ship";
    }

    public async Task<List<OrderSummary>> GetMyOrders(string userId)
    {
        return await Caller.GetAsync<List<OrderSummary>>($"{_getOrdersUrl}?userId={userId}") ?? new List<OrderSummary>();
    }

    public async Task<Order?> GetOrder(string userId, int orderNumber)
    {
        return await Caller.GetAsync<Order>($"{userId}/{orderNumber}") ?? new Order();
    }

    public async Task<bool> ShipOrder(int orderNumber)
    {
        var stringContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
        });

        var response = await Caller.PutAsync(_shipOrderUrl, stringContent);
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
        var response = await Caller.PutAsync($"{_cancelOrderUrl}/{orderNumber}", null);
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

    protected override string BaseAddress { get; set; }
}

