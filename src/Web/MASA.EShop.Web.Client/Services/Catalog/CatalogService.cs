using Microsoft.AspNetCore.WebUtilities;

namespace MASA.EShop.Web.Client.Services.Catalog;

public class CatalogService : HttpClientCaller
{
    private readonly string getCatalogItemsUrl;
    private readonly string getAllBrandsUrl;
    private readonly string getAllTypesUrl;
    private string prefix = "/api/v1/catalog/";

    public CatalogService(IServiceProvider serviceProvider, IOptions<Settings> settings) : base(serviceProvider)
    {
        Name = nameof(CatalogService);
        BaseAddress = settings.Value.ApiGatewayUrlExternal;
        getCatalogItemsUrl = $"{prefix}items";
        getAllBrandsUrl = $"{prefix}brands";
        getAllTypesUrl = $"{prefix}types";
    }

    public async Task<CatalogData> GetCatalogItemsAsync(int pageIndex, int pageSize, int brandId, int typeId)
    {
        var queryArguments = new Dictionary<string, string?>()
        {
            { "brandId", brandId.ToString() },
            { "typeId", typeId.ToString() },
            { "pageIndex", pageIndex.ToString() },
            { "pageSize", pageSize.ToString() }
        };
        var url = QueryHelpers.AddQueryString(getCatalogItemsUrl, queryArguments);
        return await CallerProvider.GetFromJsonAsync<CatalogData>(url) ?? new();
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        return await CallerProvider.GetFromJsonAsync<IEnumerable<CatalogBrand>>(getAllBrandsUrl) ?? new List<CatalogBrand>();
    }

    public async Task<IEnumerable<CatalogType>> GetTypesAsync()
    {
        return await CallerProvider.GetFromJsonAsync<IEnumerable<CatalogType>>(getAllTypesUrl) ?? new List<CatalogType>();
    }

    public async Task<CatalogItem> GetCatalogById(int Id)
    {
        return await CallerProvider.GetFromJsonAsync<CatalogItem>($"{prefix}{Id}") ?? new();
    }
}

