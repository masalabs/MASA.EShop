using MASA.EShop.Web.Client.Data.Basket;
using MASA.EShop.Web.Client.Data.Catalog;
using MASA.EShop.Web.Client.Data.Catalog.Record;
using MASA.EShop.Web.Client.Pages.Catalog.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MASA.EShop.Web.Client.Pages.Catalog
{
    public partial class Catalog : EShopBasePage
    {

        private CatalogData _catalogViewModel = new();
        private CatalogOptinsModel _catalogOptinsModel = new() { Type = -1, Brand = -1 };
        private List<CatalogBrand> _brands = new();
        private List<CatalogType> _types = new();

        [Inject] //todo :change api open
        private ICatalogService _catalogService { get; set; } = default!;

        [Inject]
        private IBasketService _baksetService { get; set; } = default!;

        [CascadingParameter]
        private Task<AuthenticationState> _authenticationStateTask { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await Task.WhenAll(
            LoadBrandsAsync(),
            LoadTypesAsync(),
            LoadItemsAsync(),
            _authenticationStateTask.ContinueWith(task =>
            {
                _isAuthenticated = task.Result.User.Identity.IsAuthenticated;
            }));
        }

        private async Task LoadBrandsAsync()
        {
            var brands = new List<CatalogBrand>
        {
            new CatalogBrand(-1,"All")
        };

            try
            {
                brands.AddRange(await _catalogService.GetBrandsAsync());
            }
            catch (Exception e)
            {
                // If call fails, we'll just have the 'All' selection.
            }

            _brands = brands;
        }

        private async Task LoadTypesAsync()
        {
            var types = new List<CatalogType>
        {
            new CatalogType(-1,"All")
        };

            try
            {
                types.AddRange(await _catalogService.GetTypesAsync());
            }
            catch
            {
                // If call fails, we'll just have the 'All' selection.
            }

            _types = types;
        }

        private async Task OnPageIndexChangedAsync(int newPageIndex)
        {
            _catalogOptinsModel.PageIndex = newPageIndex;
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                _catalogViewModel = await _catalogService.GetCatalogItemsAsync(_catalogOptinsModel.PageIndex, 9, _catalogOptinsModel.Brand, _catalogOptinsModel.Type);
            }
            catch (Exception ex)
            {
                Message(ex.Message, BlazorComponent.AlertTypes.Error);
            }
        }

        private async void AddToCart(CatalogItem item)
        {
            if (_isAuthenticated)
        {
                await _baksetService.AddItemToBasketAsync("masa", item.Id);
            Navigation("basket");
        }
            else
            {
                Navigation("/");
            }
        }
    }
}
