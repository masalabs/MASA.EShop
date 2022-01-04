using MASA.Blazor;

namespace MASA.EShop.Web.Client.Pages.Basket;

public partial class Checkout : ComponentBase
{
    private bool _valid = true;
    private MForm _form = default!;
    private ShipAddressViewModel _shipAddressViewModel = new(), _shipAddressFormModel = new();
    [Inject]
    private BasketService BaksetService { get; set; } = default!;

    [Parameter]
    public StringNumber Step { get; set; } = default!;

    [Parameter]
    public EventCallback<StringNumber> StepChanged { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public EventCallback<bool> DisabledChanged { get; set; }

    [CascadingParameter]
    private Basket Basket { get; set; } = default!;

    public Localizer T { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        T = Basket.T;
        if (Basket.IsAuthenticated)
        {
            var claims = Basket.User.Claims.ToDictionary(c => c.Type, c => c.Value);
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
                Buyer = Basket.User?.Identity?.Name ?? "",
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
                Buyer = Basket.User?.Identity?.Name ?? "",
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

    private async Task SubmitOrder()
    {
        await _form.ValidateAsync();
        if (_valid)
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

            await BaksetService.CheckoutAsync(basketCheckout);
            await StepChanged.InvokeAsync(2);
            await DisabledChanged.InvokeAsync(false);
        }
        catch (Exception e)
        {
            Basket.Message(e.Message, AlertTypes.Error);
        }
    }
}

