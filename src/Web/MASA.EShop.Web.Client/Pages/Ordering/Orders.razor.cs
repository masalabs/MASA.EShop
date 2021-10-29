namespace MASA.EShop.Web.Client.Pages.Ordering;

[Authorize]
public partial class Orders : EShopPageBase, IAsyncDisposable
{

    private HubConnection hubConnection;
    private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "ORDER NUMBER", "DATE", "TOTAL", "STATUS", "" };
    private bool _loading = false;
    private List<OrderSummary> _orders = new();

    [Inject]
    private IOrderService _orderService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (IsAuthenticated)
        {
            await LoadOrders();
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Settings.Value.OrderingUrl}/hub/notificationhub",
                    HttpTransportType.WebSockets | HttpTransportType.LongPolling, options =>
                    {
                        options.AccessTokenProvider = () =>
                        {
                            return Task.FromResult("masa");
                        };
                    }
                )
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole();
                })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<string, string>("UpdatedOrderState", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                StateHasChanged();
            });

            await hubConnection.StartAsync();

        }
    }

    private async Task LoadOrders()
    {
        _orders = await _orderService.GetMyOrders(User.Identity.Name);
    }

    private async Task CancelOrder(int orderNumber)
    {
        try
        {
            await _orderService.CancelOrder(orderNumber);
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}

