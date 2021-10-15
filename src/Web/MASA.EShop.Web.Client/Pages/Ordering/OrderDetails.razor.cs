using MASA.EShop.Web.Client.Data.Order.Record;
using Microsoft.AspNetCore.Components;

namespace MASA.EShop.Web.Client.Pages.Ordering
{
    public partial class OrderDetails : EShopBasePage
    {
        private bool _loading = false;
        private Order _order;

        [Parameter]
        public int OrderNumber { get; set; }

        protected override Task OnInitializedAsync()
        {
            _order = new Order(1, DateTime.Now, "status", "Des", new Address("Street", "City", "State", "Country"), new List<OrderItem>() {
                new OrderItem(1,"ProductName",1,2,"https://picsum.photos/id/11/500/300")
            });
            //try
            //{
            //    _order = await _orderClient.GetOrderDetailsAsync(OrderNumber);
            //}
            //catch (AccessTokenNotAvailableException ex)
            //{
            //    ex.Redirect();
            //}
            //catch (Exception ex)
            //{
            //    _error = ex.Message;
            //}
            return base.OnInitializedAsync();
        }
    }
}
