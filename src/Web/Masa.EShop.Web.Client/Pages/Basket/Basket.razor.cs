namespace Masa.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Basket : EShopPageBase
{
    private StringNumber _curTab = 0;
    private bool _addressDisabled = true;
    private bool _paymentDisabled = true;

    [Inject]
    private BasketService BaksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadBasketAsync();
    }

    private async Task LoadBasketAsync()
    {
        try
        {
            if (User.Identity?.Name is null)
            {
                Navigation("/login");
                return;
            }
            var userBasket = await BaksetService.GetBasketAsync(User.Identity.Name);
            if (userBasket is null)
            {
                await MessageAsync("Not Found");
                return;
            }
        }
        catch (Exception ex)
        {
            await MessageAsync(ex.Message, AlertTypes.Error);
        }
    }


}

