namespace MASA.EShop.Web.Client.Data.Basket.Record;

public record BasketItem(
        int ProductId,
        string ProductName,
        decimal UnitPrice,
        int Quantity,
        string PictureFileName)
{
    public string GetFormattedPrice() => UnitPrice.ToString("0.00");

    public string GetFormattedTotalPrice() => (UnitPrice * Quantity).ToString("0.00");

    public string GetPictureUrl() => $"./img/{PictureFileName}";
}

