namespace MASA.EShop.Api.Caller;

internal class DaprClientCaller : ServiceCaller
{
    public string AppId { get; init; } = default!;

    public DaprClientCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}

