namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Basket : EShopPageBase
{
    private UserBasket _userBasket = new UserBasket("", new List<BasketItem>());

    [Inject]
    private BasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadBasketAsync();
    }

    private async Task LoadBasketAsync()
    {
        try
        {
            //User.Identity.Name
            var userBasket = await _baksetService.GetBasketAsync("masa");
            if (userBasket is null)
            {
                Message("Not Found");
                return;
            }
            _userBasket = userBasket;
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }

    private async Task RemoveItemAsync(int productId)
    {
        try
        {
            var name = User.Identity?.Name;
            if (string.IsNullOrEmpty(name))
            {
                Navigation("/login");
                return;
            }
            await _baksetService.RemoveItemAsync(name, productId);
            await LoadBasketAsync();
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }

    private void NavToCheckout()
    {
        if (_userBasket.Items.Any())
        {
            Navigation("basket/checkout");
            return;
        }
        Message("购物车中没有商品", AlertTypes.Warning);
    }
}

