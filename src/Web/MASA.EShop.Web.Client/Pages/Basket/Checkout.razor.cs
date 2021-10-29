namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Checkout : EShopPageBase
{
    private ShipAddressViewModel _shipAddressViewModel = new();

    [Inject]
    private IBasketService _baksetService { get; set; } = default!;

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

            _shipAddressViewModel = new ShipAddressViewModel
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
        }
    }

    public async void SubmitOrder(EditContext context)
    {
        var success = context.Validate();
        if (success)
        {
            try
            {
                var basketCheckout = new BasketCheckout(
                                        _shipAddressViewModel.Street,
                                        _shipAddressViewModel.City,
                                        _shipAddressViewModel.State,
                                        _shipAddressViewModel.Country,
                                        _shipAddressViewModel.ZipCode,
                                        _shipAddressViewModel.CardNumber,
                                        _shipAddressViewModel.CardHolderName,
                                        CardExpirationDate.Parse(_shipAddressViewModel.CardExpiration),
                                        _shipAddressViewModel.CardSecurityCode, 1, _shipAddressViewModel.Buyer, Guid.NewGuid());

                await _baksetService.CheckoutAsync(basketCheckout);
                //Navigation("/basket/success");
                Navigation("orders");
            }
            catch (Exception e)
            {
                Message(e.Message, AlertTypes.Error);
            }
        }
    }
}

