namespace MASA.EShop.Web.Client
{
    public class Settings
    {
        public string ApiGatewayUrlExternal { get; set; } = default!;

        #region todo change bff api

        public string BasketUrl { get; set; } = default!;

        public string CatalogUrl { get; set; } = default!;

        public string OrderingUrl { get; set; } = default!;

        #endregion
    }
}
