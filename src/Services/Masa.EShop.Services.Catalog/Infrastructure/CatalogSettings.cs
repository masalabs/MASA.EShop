namespace Masa.EShop.Services.Catalog.Infrastructure;

public class CatalogSettings
{
    public string PicBaseUrl { get; set; } = default!;

    public bool UseCustomizationData { get; set; }

    public bool AzureStorageEnabled { get; set; }
}
