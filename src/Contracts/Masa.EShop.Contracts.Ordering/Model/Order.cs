namespace Masa.EShop.Contracts.Ordering.Model;
public record Order(
   int OrderNumber,
   DateTime Date,
   string Status,
   string Description,
   string Street,
   string City,
   string ZipCode,
   string Country,
   List<OrderItem> OrderItems)
{
    public Order() : this(0, DateTime.Now, "", "", "", "", "", "", new List<OrderItem>())
    {
    }

    public decimal Total => OrderItems.Sum(o => o.Units * o.UnitPrice);

    public string GetFormattedOrderDate() => Date.ToString("f");

    public string GetFormattedTotal() => Total.ToString("0.00");

    public string GetPictureUrl() => OrderItems.First().GetPictureUrl();

    public string ProductName
    {
        get
        {
            return OrderItems.First().ProductName;
        }
    }
}

