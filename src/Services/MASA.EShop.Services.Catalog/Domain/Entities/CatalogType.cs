
namespace MASA.EShop.Services.Catalog.Domain.Entities;
public class CatalogType
{
    public int Id { get; set; }

    public string Type { get; private set; } = null!;

    public CatalogType(int id, string type)
    {
        Id = id;
        Type = type;
    }
}
