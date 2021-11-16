namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Basket : EShopPageBase
{
    private UserBasket _userBasket = new UserBasket("", new List<BasketItem>());

    protected override string PageName { get; set; } = "Basket";

    [Inject]
    private IBasketService _baksetService { get; set; } = default!;

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
            _userBasket = await _baksetService.GetBasketAsync("masa");
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
            await _baksetService.RemoveItemAsync(User.Identity.Name, productId);
            await LoadBasketAsync();
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }
}

