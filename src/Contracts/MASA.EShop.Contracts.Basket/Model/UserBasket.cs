namespace MASA.EShop.Contracts.Basket.Model.Web;

public record UserBasket(string BuyerId, List<BasketItem> Items)
{
    public decimal Total()
    {
        return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }

    public string GetFormattedTotalPrice() => Items.Sum(
        item => item.Quantity * item.UnitPrice).ToString("0.00");
}

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
