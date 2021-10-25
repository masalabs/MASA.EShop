namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Basket : EShopBasePage
{
    private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "PRODUCT", "", "PRICE", "QUANTITY", "COST" };
    private UserBasket _userBasket = new UserBasket("", new List<BasketItem>());

    private bool _loading = false;

    [Inject]
    private IBasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
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

}

