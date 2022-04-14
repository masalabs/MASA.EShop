namespace Masa.EShop.Api.Open.Services;

public class CatalogService : ServiceBase
{
    private readonly CatalogCaller _catalogCaller;

    public CatalogService(IServiceCollection services, CatalogCaller catalogCaller) : base(services)
    {
        _catalogCaller = catalogCaller;
        App.MapGet("/api/v1/catalog/{id}", GetAsync).Produces<CatalogItem>();
        App.MapGet("/api/v1/catalog/items", GetItemsAsync);
        App.MapGet("/api/v1/catalog/brands", GetCatalogBrandsAsync);
        App.MapGet("/api/v1/catalog/types", GetCatalogTypesAsync);
    }

    public async Task<IResult> GetAsync(int id)
    {
        return Results.Ok(await _catalogCaller.GetCatalogById(id));
    }

    public async Task<IResult> GetItemsAsync(int typeId = -1, int brandId = -1, int pageSize = 10, int pageIndex = 0)
    {
        var data = await _catalogCaller.GetCatalogItemsAsync(pageIndex, pageSize, brandId, typeId);
        if (data is null)
        {
            return Results.BadRequest("No Data");
        }
        return Results.Ok(data);
    }

    public async Task<IResult> GetCatalogBrandsAsync()
    {
        return Results.Ok(await _catalogCaller.GetBrandsAsync());
    }

    public async Task<IResult> GetCatalogTypesAsync()
    {
        return Results.Ok(await _catalogCaller.GetTypesAsync());
    }

}

