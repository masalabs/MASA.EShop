namespace MASA.EShop.Services.Catalog.Service;
public class CatalogService : ServiceBase
{
    public CatalogService(IServiceCollection services)
        : base(services)
    {
        App.MapGet("/api/v1/catalog/{id}", Get);
    }

    public Task<dynamic> Get(int id)
    {
        throw new NotImplementedException();
    }
}
