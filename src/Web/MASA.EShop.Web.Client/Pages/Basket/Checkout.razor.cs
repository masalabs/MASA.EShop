using BlazorComponent;
using MASA.EShop.Web.Client.Data.Basket.Record;

namespace MASA.EShop.Web.Client.Pages.Basket
{
    public partial class Checkout : EShopBasePage
    {
        private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "PRODUCT", "", "PRICE", "QUANTITY", "COST" };
        private UserBasket _userBasket = new("", new List<BasketItem> {
                new BasketItem(1,"Test Product Name",21,2,"https://picsum.photos/id/11/500/300")
            }); //todo get CustomerBasket

        private bool _loading = false;
    }
}
