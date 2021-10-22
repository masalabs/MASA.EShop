namespace MASA.EShop.Web.Client.Data.Catalog.Record;

public class CatalogData
{
    public int Count { get; set; }

    public IEnumerable<CatalogItem> Items { get; set; } = Enumerable.Empty<CatalogItem>();

    public int PageIndex { get; set; }

    public int PageSize { get; set; } = 1;

    public int PageCount => (int)Math.Ceiling(Count / (decimal)PageSize);
}

