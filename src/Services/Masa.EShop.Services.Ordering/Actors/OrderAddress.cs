namespace Masa.EShop.Services.Ordering.Actors;

public class OrderAddress
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
}

