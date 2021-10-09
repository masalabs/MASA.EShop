namespace MASA.EShop.Services.Catalog.Service;

public class CatalogService : ServiceBase
{
    public CatalogService(IServiceCollection services)
        : base(services)
    {
        App.MapGet("/api/v1/catalog/{id}", Get);
        App.MapPost("/api/v1/catalog/createproduct", CreateProductAsync);
        App.MapPost("/api/v1/catalog/createcatalogtype", CreateCatalogTypeAsync);
    }

    public Task<dynamic> Get(int id)
    {
        throw new NotImplementedException();
    }

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
}
