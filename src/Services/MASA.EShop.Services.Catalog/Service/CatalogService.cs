
namespace MASA.EShop.Services.Catalog.Service;
public class CatalogService
{
    public CatalogService(WebApplication app)
    {
        app.MapGet("/api/v1/catalog/{id}", Get);
    }

    public Task<dynamic> Get(int id)
    {
        throw new NotImplementedException();
    }
}
