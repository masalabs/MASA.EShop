namespace Masa.EShop.Web.Client.Services.Ordering;

public class OrderService : HttpClientCallerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string getOrdersUrl = "";
    private readonly string cancelOrderUrl = "";
    private readonly string shipOrderUrl = "";
    private readonly string prefix = "/api/v1/orders/";

    public OrderService(IServiceProvider serviceProvider, IOptions<Settings> settings) : base(serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Name = nameof(OrderService);
        BaseAddress = settings.Value.ApiGatewayUrlExternal;
        getOrdersUrl = $"{prefix}list";
        cancelOrderUrl = $"{prefix}cancel";
        shipOrderUrl = $"{prefix}ship";
    }

    protected override IHttpClientBuilder UseHttpClient()
    {
        return base.UseHttpClient().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
    }

    public async Task<List<OrderSummary>> GetMyOrders(string userId)
    {
        return await CallerProvider.GetAsync<List<OrderSummary>>($"{getOrdersUrl}?userId={userId}") ?? new List<OrderSummary>();
    }

    public async Task<Order?> GetOrder(string userId, int orderNumber)
    {
        return await CallerProvider.GetAsync<Order>($"{prefix}{userId}/{orderNumber}") ?? new Order();
    }

    public async Task ShipOrder(int orderNumber)
    {
        var order = new
        {
            OrderNumber = orderNumber
        };

        var stringContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
        });

        var response = await CallerProvider.PutAsync(shipOrderUrl, stringContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelOrder(int orderNumber)
    {
        var response = await CallerProvider.PutAsync($"{cancelOrderUrl}/{orderNumber}", null);
        response.EnsureSuccessStatusCode();
    }

    protected override string BaseAddress { get; set; }
}

