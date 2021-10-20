using MASA.EShop.Web.Client.Data.Basket.Record;

namespace MASA.EShop.Web.Client.Data.Basket
{
    public interface IBasketService
    {
        Task<UserBasket> GetBasketAsync(string userId);
        Task AddItemToBasketAsync(string userId, int productId);
        Task<UserBasket> UpdateBasketAsync(UserBasket basket);
        //Task Checkout(BasketDTO basket);
    }
}
