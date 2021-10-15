using BlazorComponent;
using MASA.EShop.Web.Client.Data.Order.Record;

namespace MASA.EShop.Web.Client.Pages.Ordering
{
    public partial class Orders : EShopBasePage
    {
        private readonly List<TableHeaderOptions> _headers = new List<TableHeaderOptions> { "ORDER NUMBER", "DATE", "TOTAL", "STATUS", "" };
        private bool _loading = false;
        private List<OrderSummary> _orders = new()
        {
            new OrderSummary(Guid.NewGuid(), 1, DateTime.Now, "Submitted", 1),
            new OrderSummary(Guid.NewGuid(), 1, DateTime.Now, "Paied", 2)
        };

    }
}
