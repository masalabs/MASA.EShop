namespace MASA.EShop.Web.Client.Data.Ordering.ViewModel;

public class ShipAddressViewModel
{
    [Required]
    public string Buyer { get; set; } = default!;

    [Required]
    public string Street { get; set; } = default!;

    [Required]
    public string City { get; set; } = default!;

    [Required]
    public string Country { get; set; } = default!;

    public string? ZipCode { get; set; }

    [Required]
    public string State { get; set; } = default!;

    [Required]
    public string CardNumber { get; set; } = default!;

    [Required]
    public string CardHolderName { get; set; } = default!;

    [Required]
    [CustomValidation(typeof(CardExpirationDate), "Validate")]
    public string CardExpiration { get; set; } = default!;

    [Required]
    public string CardSecurityCode { get; set; } = default!;
}

