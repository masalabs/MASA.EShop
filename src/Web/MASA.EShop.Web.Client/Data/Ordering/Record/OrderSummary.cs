namespace MASA.EShop.Web.Client.Data.Ordering.Record;

public record OrderSummary(
        Guid Id,
        int OrderNumber,
        DateTime Date,
        string Status,
        decimal Total)
{
    public string GetFormattedOrderDate() => Date.ToString("d");

    public string GetFormattedTotal() => Total.ToString("0.00");
}

