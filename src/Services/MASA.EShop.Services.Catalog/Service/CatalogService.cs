namespace MASA.EShop.Services.Catalog.Service;

public class CatalogService : ServiceBase
{
    public CatalogService(IServiceCollection services)
        : base(services)
    {
        App.MapGet("/api/v1/catalog/{id}", GetAsync);
        App.MapGet("/api/v1/catalog/items", GetItemsAsync);
        App.MapGet("/api/v1/catalog/brands", CatalogBrandsAsync);
        App.MapGet("/api/v1/catalog/types", CatalogTypesAsync);
        App.MapPost("/api/v1/catalog/createproduct", CreateProductAsync);
        App.MapPost("/api/v1/catalog/createcatalogtype", CreateCatalogTypeAsync);

    }

    #region Query

    public async Task<IResult> GetAsync([FromServices] IEventBus eventBus, int id)
    {
        var query = new ProductQuery() { ProductId = id };
        await eventBus.PublishAsync(query);
        return Results.Ok(CatalogItemDto.FromOrderItem(query.Result));
    }

    public async Task<IResult> GetItemsAsync([FromServices] IEventBus eventBus,
            [FromQuery] int typeId = -1,
            [FromQuery] int brandId = -1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0)
    {
        var query = new ProductsQuery()
        {
            TypeId = typeId,
            BrandId = brandId,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
        await eventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    public async Task<IResult> CatalogBrandsAsync([FromServices] IEventBus eventBus)
    {
        var query = new CatalogBrandsQuery();
        await eventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    public async Task<IResult> CatalogTypesAsync([FromServices] IEventBus eventBus)
    {
        var query = new CatalogTypesQuery();
        await eventBus.PublishAsync(query);
        return Results.Ok(query.Result);
    }

    #endregion

    #region Command

    public async Task<IResult> CreateProductAsync(
        CreateProductCommand command,
        [FromServices] IEventBus eventBus)
    {
        await eventBus.PublishAsync(command);
        return Results.Accepted();
    }

    public async Task<IResult> CreateCatalogTypeAsync(
        CreateCatalogTypeCommand command,
        [FromServices] IEventBus eventBus)
    {
        await eventBus.PublishAsync(command);
        return Results.Accepted();
    }

    public Task<ActionResult> DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    #endregion
}
