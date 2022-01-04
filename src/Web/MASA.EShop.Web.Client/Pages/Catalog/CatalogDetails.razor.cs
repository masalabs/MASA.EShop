namespace MASA.EShop.Web.Client.Pages.Catalog;

public partial class CatalogDetails : EShopPageBase
{
    private CatalogItem _catalogItem = new();
    private List<RelatedProduct> _relatedProducts = new()
    {
        new() { Name = "GA406B 温热管线饮水机", Brand = "Lonsid", ImgUrl = "https://img-cdn.lonsid.co/image/1593360117.jpg", Price = 9999, Rating = 5 },
        new() { Name = "G1管线饮水机", Brand = "Lonsid", ImgUrl = "https://img-cdn.lonsid.co/image/1593360094.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GA406K 速热管线饮水机", Brand = "Lonsid", ImgUrl = "https://img-cdn.lonsid.co/image/1555404598.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GT3桌面即热饮水机", Brand = "Lonsid", ImgUrl = "https://img-cdn.lonsid.co/image/1560130226.jpg", Price = 9999, Rating = 5 },
        new() { Name = "GR320RB冷热型饮水机", Brand = "Lonsid", ImgUrl = "https://img-cdn.lonsid.co/image/1603728000ddBMgnpYFmMWTlAl3bAX179.jpg", Price = 9999, Rating = 5 }
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

