namespace MASA.EShop.Web.Client.Data.Ordering.Record
{
    public record OrderSummary(
        Guid Id,
        int OrderNumber,
        DateTime OrderDate,
        string OrderStatus,
        decimal Total)
    {
        public string GetFormattedOrderDate() => OrderDate.ToString("d");

        public string GetFormattedTotal() => Total.ToString("0.00");
    }
}
