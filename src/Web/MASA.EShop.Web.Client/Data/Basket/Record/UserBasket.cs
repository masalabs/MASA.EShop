namespace MASA.EShop.Web.Client.Data.Basket.Record
{
    public record UserBasket(string BuyerId, List<BasketItem> Items)
    {
        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }

        public string GetFormattedTotalPrice() => Items.Sum(
            item => item.Quantity * item.UnitPrice).ToString("0.00");
    }
}
