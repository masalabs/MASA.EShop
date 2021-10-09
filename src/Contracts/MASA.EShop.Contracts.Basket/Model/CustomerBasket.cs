namespace MASA.EShop.Contracts.Basket.Model;

public class CustomerBasket
{
    public string BuyerId { get; set; } = default!;

    public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    public CustomerBasket()
    {

    }

    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
    }
}
