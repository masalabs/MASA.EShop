namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Payment : EShopPageBase
{
    private int _payOptions = 1;

    [Inject]
    private BasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

    }
}

