namespace MASA.EShop.Web.Client.Data.Ordering.ViewModel;

public class ShipAddressViewModel
{
    [Required]
    public string Buyer { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    public string ZipCode { get; set; }

    [Required]
    public string State { get; set; }

    [Required]
    public string CardNumber { get; set; }

    [Required]
    public string CardHolderName { get; set; }

    [Required]
    [CustomValidation(typeof(CardExpirationDate), "Validate")]
    public string CardExpiration { get; set; }

    [Required]
    public string CardSecurityCode { get; set; }
}

