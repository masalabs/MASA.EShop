namespace Masa.EShop.Contracts.Ordering.Model;

public record OrderSummary(
        Guid Id,
        string ProductName,
        string PictureName,
        int OrderNumber,
        DateTime Date,
        string Status,
        decimal Total)
{
    public string GetFormattedOrderDate() => Date.ToString("d");

    public string GetFormattedTotal() => Total.ToString("0.00");

    public string GetPictureUrl() => $"./img/{PictureName}";
}

