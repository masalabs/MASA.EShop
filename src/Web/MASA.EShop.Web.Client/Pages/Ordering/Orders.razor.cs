using BlazorComponent;
using MASA.EShop.Web.Client.Data.Ordering;
using MASA.EShop.Web.Client.Data.Ordering.Record;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace MASA.EShop.Web.Client.Pages.Ordering
{
    [Authorize]
    public partial class Orders : EShopBasePage
    {

        private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "ORDER NUMBER", "DATE", "TOTAL", "STATUS", "" };
        private bool _loading = false;
        private List<OrderSummary> _orders = new();

        [Inject]
        private IOrderService _orderService { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            try
            {
                _orders = await _orderService.GetMyOrders("masa");
            }
            catch (Exception ex)
            {
                Message(ex.Message, AlertTypes.Error);
            }
        }

        private async Task CancelOrder(int orderNumber)
        {
            try
            {
                await _orderService.CancelOrder(orderNumber);
            }
            catch (Exception ex)
        {
                Message(ex.Message, AlertTypes.Error);
            }
        }

    }
}
