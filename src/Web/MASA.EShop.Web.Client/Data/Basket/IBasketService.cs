namespace MASA.EShop.Web.Client.Data.Basket;

public interface IBasketService
{
    Task<UserBasket> GetBasketAsync(string userId);
    Task AddItemToBasketAsync(string userId, int productId);
    Task<UserBasket> UpdateBasketAsync(UserBasket basket);
    Task CheckoutAsync(BasketCheckout basketCheckout);
    Task RemoveItemAsync(string userId, int productId);
}

