namespace Masa.EShop.Web.Client.Pages.Catalog;

public partial class CatalogDetails : EShopPageBase
{
    private CatalogItem _catalogItem = new();
    private List<RelatedProduct> _relatedProducts = new()
    {
        new() { Name = "GA406B 温热管线饮水机", Brand = "Lonsid", ImgUrl = "./img/15.jpg", Price = 9999, Rating = 5 },
        new() { Name = "G1管线饮水机", Brand = "Lonsid", ImgUrl = "./img/16.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GA406K 速热管线饮水机", Brand = "Lonsid", ImgUrl = "./img/15.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GT3桌面即热饮水机", Brand = "Lonsid", ImgUrl = "./img/17.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GR320RB冷热型饮水机", Brand = "Lonsid", ImgUrl = "./img/15.jpg", Price = 9999, Rating = 5 }
    };

    [Parameter]
    public int Id { get; set; }

    [Inject]
    private CatalogService CatalogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _catalogItem = await CatalogService.GetCatalogById(Id);
    }
}

