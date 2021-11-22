using MASA.EShop.Contracts.Catalog.Model;
using MASA.EShop.Web.Client.Services.Catalog;

namespace MASA.EShop.Web.Client.Pages.Catalog
{
    public partial class CatalogDetails : EShopPageBase
    {

        private CatalogItem _catalogItem = new();

        protected override string PageName { get; set; } = "Catalog";

        [Parameter]
        public int Id { get; set; }

        [Inject] //todo :change api open
        private CatalogService _catalogService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _catalogItem = await _catalogService.GetCatalogById(Id);
        }

    }
}
