namespace MASA.EShop.Web.Client.Pages.Catalog.ViewModel;

public class CatalogViewModel
{
    public int Count { get; set; }

    public IEnumerable<CatalogItem> Items { get; set; } = Enumerable.Empty<CatalogItem>();

    public int PageIndex { get; set; }

    public int PageSize { get; set; } = 1;

    public int PageCount => (int)Math.Ceiling(Count / (decimal)PageSize);
}

