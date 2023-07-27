namespace Masa.EShop.Web.Client.Pages.Catalog;

public partial class Catalog : EShopPageBase
{

    private CatalogData _catalogViewModel = new();
    private CatalogOptinsModel _catalogOptinsModel = new() { Type = -1, Brand = -1 };
    private List<CatalogBrand> _brands = new();
    private List<CatalogType> _types = new();
    private List<string> SortItems = new()
    {
        "Featured",
        "Lowest",
        "Highest"
    };
    private bool _show = false;

    [Inject]
    private CatalogService CatalogService { get; set; } = default!;

    [Inject]
    private BasketService BaksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Task.WhenAll(
            LoadBrandsAsync(),
            LoadTypesAsync(),
            LoadItemsAsync()
        );
    }

    private async Task LoadBrandsAsync()
    {
        var brands = new List<CatalogBrand>
            {
                new CatalogBrand(-1,"全部")
            };

        try
        {
            brands.AddRange(await CatalogService.GetBrandsAsync());
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
                new CatalogType(-1,"全部")
            };

        try
        {
            types.AddRange(await CatalogService.GetTypesAsync());
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
            _catalogViewModel = await CatalogService.GetCatalogItemsAsync(_catalogOptinsModel.PageIndex, 9, _catalogOptinsModel.Brand, _catalogOptinsModel.Type);
        }
        catch (Exception ex)
        {
            await MessageAsync(ex.Message, AlertTypes.Error);
        }
    }

    private async void AddToCart(CatalogItem item)
    {
        if (IsAuthenticated)
        {
            await BaksetService.AddItemToBasketAsync("masa", item.Id);
            Navigation("basket");
        }
        else
        {
            Navigation("/");
        }
    }
}

