using BlazorComponent;
using MASA.EShop.Web.Client.Data.Basket;
using MASA.EShop.Web.Client.Data.Basket.Record;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace MASA.EShop.Web.Client.Pages.Basket
{
    [Authorize]
    public partial class Basket : EShopBasePage
    {
        private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "PRODUCT", "", "PRICE", "QUANTITY", "COST" };
        private UserBasket _userBasket = default!;

        private bool _loading = false;

        [Inject]
        private IBasketService _baksetService { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            try
            {
                _userBasket = await _baksetService.GetBasketAsync("masa");
            }
            catch (Exception ex)
            {
                Message(ex.Message, AlertTypes.Error);
            }
        }
    }
}
