using Microsoft.AspNetCore.WebUtilities;

namespace MASA.EShop.Api.Open.Callers.Catalog;

public class CatalogCaller : HttpClientCaller
{
    private readonly ILogger<CatalogCaller> _logger;

    private readonly string _getCatalogItemsUrl;
    private readonly string _getAllBrandsUrl;
    private readonly string _getAllTypesUrl;
    private string prefix = "/api/v1/catalog/";

    public CatalogCaller(
        IServiceProvider serviceProvider,
        IOptions<Settings> settings,
        ILogger<CatalogCaller> logger) : base(serviceProvider)
    {
        BaseAddress = settings.Value.CatalogUrl;
        Name = nameof(CatalogCaller);
        _logger = logger;
        _getCatalogItemsUrl = $"{prefix}items";
        _getAllBrandsUrl = $"{prefix}brands";
        _getAllTypesUrl = $"{prefix}types";
    }

    public async Task<CatalogData?> GetCatalogItemsAsync(int pageIndex, int pageSize, int brandId = -1, int typeId = -1)
    {
        var queryArguments = new Dictionary<string, string?>()
        {
            { "brandId", brandId.ToString() },
            { "typeId", typeId.ToString() },
            { "pageIndex", pageIndex.ToString() },
            { "pageSize", pageSize.ToString() }
        };
        var url = QueryHelpers.AddQueryString(_getCatalogItemsUrl, queryArguments);
        return await CallerProvider.GetFromJsonAsync<CatalogData>(url);
    }
    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        return await CallerProvider.GetFromJsonAsync<IEnumerable<CatalogBrand>>(_getAllBrandsUrl) ?? new List<CatalogBrand>();
    }

    public async Task<IEnumerable<CatalogType>> GetTypesAsync()
    {
        return await CallerProvider.GetFromJsonAsync<IEnumerable<CatalogType>>(_getAllTypesUrl) ?? new List<CatalogType>();
    }

    public async Task<CatalogItem?> GetCatalogById(int Id)
    {
        return await CallerProvider.GetFromJsonAsync<CatalogItem>($"{prefix}{Id}");
    }
}

