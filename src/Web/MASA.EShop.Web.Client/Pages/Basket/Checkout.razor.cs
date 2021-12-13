namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Checkout : EShopPageBase
{
    private ShipAddressViewModel _shipAddressViewModel = new(), _shipAddressFormModel = new();

    [Inject]
    private BasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (IsAuthenticated)
        {
            var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
            claims.TryGetValue("address_street", out string? address_street);
            claims.TryGetValue("address_city", out string? address_city);
            claims.TryGetValue("address_state", out string? address_state);
            claims.TryGetValue("address_country", out string? address_country);
            claims.TryGetValue("card_number", out string? card_number);
            claims.TryGetValue("card_holder", out string? card_holder);
            claims.TryGetValue("card_expiration", out string? card_expiration);
            claims.TryGetValue("card_security_number", out string? card_security_number);

            _shipAddressFormModel = new ShipAddressViewModel
            {
                Buyer = User?.Identity?.Name ?? "",
                Street = address_street ?? "",
                City = address_city ?? "",
                State = address_state ?? "",
                Country = address_country ?? "",
                CardNumber = card_number ?? "",
                CardHolderName = card_holder ?? "",
                CardExpiration = card_expiration ?? "",
                CardSecurityCode = card_security_number ?? ""
            };

            _shipAddressViewModel = new ShipAddressViewModel
            {
                Buyer = User?.Identity?.Name ?? "",
                Street = "Science Park Road",
                City = "Hang Zhou",
                ZipCode = "23333",
                State = "Jianggan District",
                Country = "China",
                CardNumber = "1111-1111-22223-458",
                CardHolderName = "Masa",
                CardExpiration = "12/12",
                CardSecurityCode = "314"
            };
        }
    }

    private async Task SubmitOrder(EditContext context)
    {
        var success = context.Validate();
        if (success)
        {
            await BasketCheckout(_shipAddressFormModel);
        }
    }

    private async Task SubmitShipedOrder()
    {
        await BasketCheckout(_shipAddressViewModel);
    }

    private async Task BasketCheckout(ShipAddressViewModel model)
    {
        try
        {
            var basketCheckout = new BasketCheckout(
                                    model.Street,
                                    model.City,
                                    model.State,
                                    model.Country,
                                    model.ZipCode,
                                    model.CardNumber,
                                    model.CardHolderName,
                                    CardExpirationDate.Parse(model.CardExpiration),
                                    model.CardSecurityCode, 1, model.Buyer, Guid.NewGuid());

            await _baksetService.CheckoutAsync(basketCheckout);
            Navigation("/basket/payment");
        }
        catch (Exception e)
        {
            Message(e.Message, AlertTypes.Error);
        }
    }
}

