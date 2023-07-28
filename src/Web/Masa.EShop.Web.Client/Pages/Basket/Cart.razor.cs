namespace Masa.EShop.Web.Client.Pages.Basket;

public partial class Cart : ComponentBase
{
    [Parameter]
    public StringNumber Step { get; set; } = default!;
    [Parameter]
    public EventCallback<StringNumber> StepChanged { get; set; }
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public EventCallback<bool> DisabledChanged { get; set; }

    public Localizer T { get; set; } = default!;

    [CascadingParameter]
    protected Basket Basket { get; set; } = default!;

    private UserBasket _userBasket = new("", new List<BasketItem>());

    [Inject]
    private BasketService BaksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        T = Basket.T;
        await base.OnInitializedAsync();
        await LoadBasketAsync();
    }

    private async Task LoadBasketAsync()
    {
        try
        {
            var name = Basket.User.Identity?.Name;
            if (name is null)
            {
                Basket.Navigation("/login");
                return;
            }
            var userBasket = await BaksetService.GetBasketAsync(name);
            if (userBasket is null)
            {
                await Basket.MessageAsync("Not Found", AlertTypes.Warning);
                return;
            }
            _userBasket = userBasket;
        }
        catch (Exception ex)
        {
            await Basket.MessageAsync(ex.Message, AlertTypes.Error);
        }
    }

    private async Task RemoveItemAsync(int productId)
    {
        try
        {
            var name = Basket.User.Identity?.Name;
            if (name is null)
            {
                Basket.Navigation("/login");
                return;
            }
            await BaksetService.RemoveItemAsync(name, productId);
            await LoadBasketAsync();
        }
        catch (Exception ex)
        {
            await Basket.MessageAsync(ex.Message, AlertTypes.Error);
        }
    }

    private async void NavToCheckout()
    {
        if (!_userBasket.Items.Any())
        {
            await Basket.MessageAsync("购物车中没有商品", AlertTypes.Warning);
            return;
        }
        await StepChanged.InvokeAsync(1);
        await DisabledChanged.InvokeAsync(false);
    }
}

