namespace Masa.EShop.Contracts.Basket.Model.Web;

public record UserBasket(string BuyerId, List<BasketItem> Items)
{
    public decimal Total()
    {
        return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }

    public string GetFormattedTotalPrice() => Items.Sum(
        item => item.Quantity * item.UnitPrice).ToString("0.00");
}

public class BasketItem
{
    public BasketItem(int productId,
        string productName,
        decimal unitPrice,
        int quantity,
        string pictureFileName)
    {
        this.ProductId = productId;
        this.ProductName = productName;
        this.UnitPrice = unitPrice;
        this.Quantity = quantity;
        this.PictureFileName = pictureFileName;
    }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public string PictureFileName { get; set; }

    public string Delivery { get; set; } = "Delivery by Sun, Nov 28";

    public string Offers { get; set; } = "12% off 3 offers Available";

    public string GetFormattedPrice() => UnitPrice.ToString("0.00");

    public string GetFormattedTotalPrice() => (UnitPrice * Quantity).ToString("0.00");

    public string GetPictureUrl() => $"./img/{PictureFileName}";
}
