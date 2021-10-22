namespace MASA.EShop.Web.Client.Data.Catalog;

public interface ICatalogService
{
    Task<CatalogData> GetCatalogItemsAsync(int pageIndex, int pageSize, int? brand, int? type);
    Task<IEnumerable<CatalogBrand>> GetBrandsAsync();
    Task<IEnumerable<CatalogType>> GetTypesAsync();
    Task<CatalogItem> GetCatalogById(int Id);
}

