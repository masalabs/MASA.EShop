namespace MASA.EShop.Web.Client.Pages.Ordering;

[Authorize]
public partial class Orders : EShopPageBase, IAsyncDisposable
{

    private HubConnection hubConnection;
    private readonly List<DataTableHeader<OrderSummary>> _headers = new List<DataTableHeader<OrderSummary>> {
        new (){ Text= "IMAGE",Sortable= false,Value= nameof(OrderSummary.PictureName)},
        new (){ Text= "PRODUCT NAME",Sortable= false,Align= "center",Value= nameof(OrderSummary.ProductName)},
        new (){ Text= "ORDER NUMBER", Align= "center",Sortable= false,Value= nameof(OrderSummary.OrderNumber)},
        new (){ Text= "TOTAL", Value= nameof(OrderSummary.Total),Align= "end"},
        new (){ Text= "DATE", Align= "center",Value= nameof(OrderSummary.Date)},
        new (){ Text= "STATUS",Sortable= false,Align= "center", Value= nameof(OrderSummary.Status)},
        new (){ Text= "",Sortable= false, Value= nameof(OrderSummary.Id)}
    };
    private bool _loading = false;
    private List<OrderSummary> _orders = new();
    private Order _order = new Order(0, DateTime.MinValue, "", "", "", "", "", "", new List<OrderItem>() {
                new OrderItem(0,"",0,0,"")
            });
    private bool _detailDialog = false;

    protected override string PageName { get; set; } = "Order";

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

    private async void ShowDetails(int orderNumber)
    {
        _detailDialog = true;
        _order = await _orderService.GetOrder(User.Identity.Name, orderNumber);
    }

    private string GetStatusColor(string status)
    {
        var color = "";
        switch (status)
        {
            case "Cancelled":
                color = "green";
                break;
            case "Submitted":
                color = "blue";
                break;
            case "Shipped":
                color = "orange";
                break;
            case "Paid":
                color = "red";
                break;
        }
        return color;
    }
}
