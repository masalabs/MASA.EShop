using MASA.EShop.Web.Client.Data.Catalog.Record;
using MASA.EShop.Web.Client.Pages.Catalog.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MASA.EShop.Web.Client.Pages.Catalog
{
    public partial class Catalog : EShopBasePage
    {

        private CatalogViewModel _catalogViewModel = new();
        private CatalogOptinsModel _catalogOptinsModel = new() { Type = -1, Brand = -1 };
        private List<CatalogBrand> _brands = new();
        private List<CatalogType> _types = new();

        [CascadingParameter]
        private Task<AuthenticationState> _authenticationStateTask { get; set; }

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
                //brands.AddRange(await _catalogClient.GetBrandsAsync());
            }
            catch
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
                //types.AddRange(await _catalogClient.GetTypesAsync());
            }
            catch
            {
                // If call fails, we'll just have the 'All' selection.
            }

            _types = types;
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                //_catalogViewModel = await _catalogClient.GetItemsAsync(_brandId, _typeId, _pageIndex);
                //_error = null;
            }
            catch (Exception ex)
            {
                Message(ex.Message, BlazorComponent.AlertTypes.Error);
            }
        }

        private void AddToCart()
        {
            Navigation("basket");
        }
    }
}
