using MASA.EShop.Web.Client.Data.Catalog.Record;
using Microsoft.Extensions.Options;

namespace MASA.EShop.Web.Client.Data.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatalogService> _logger;

        private readonly string getCatalogItemsUrl;
        private readonly string getAllBrandsUrl;
        private readonly string getAllTypesUrl;
        private string party = "/api/v1/catalog/";

        public CatalogService(HttpClient httpClient, IOptions<Settings> settings, ILogger<CatalogService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
            _logger = logger;


            getCatalogItemsUrl = $"{party}items";
            getAllBrandsUrl = $"{party}brands";
            getAllTypesUrl = $"{party}types";
        }

        public async Task<CatalogData> GetCatalogItemsAsync(int pageIndex, int pageSize, int? brandId, int? typeId)
        {
            return await _httpClient.GetFromJsonAsync<CatalogData>(
            $"{getCatalogItemsUrl}?brandId={brandId}&typeId={typeId}&pageIndex={pageIndex}&pageSize={pageSize}") ?? new();
        }
        public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CatalogBrand>>(getAllBrandsUrl) ?? new List<CatalogBrand>();
        }

        public async Task<IEnumerable<CatalogType>> GetTypesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CatalogType>>(getAllTypesUrl) ?? new List<CatalogType>();
        }

        public async Task<CatalogItem> GetCatalogById(int Id)
    {
            return await _httpClient.GetFromJsonAsync<CatalogItem>($"{party}{Id}") ?? new();
        }
    }
}
