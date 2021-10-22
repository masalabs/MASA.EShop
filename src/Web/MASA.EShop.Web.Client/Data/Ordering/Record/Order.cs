namespace MASA.EShop.Web.Client.Data.Ordering.Record;
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
    public decimal Total => OrderItems.Sum(o => o.Units * o.UnitPrice);

    public string GetFormattedOrderDate() => Date.ToString("d");

    public string GetFormattedTotal() => Total.ToString("0.00");
}

