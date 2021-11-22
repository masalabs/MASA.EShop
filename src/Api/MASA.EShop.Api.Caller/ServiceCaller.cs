namespace MASA.EShop.Api.Caller;

public abstract class ServiceCaller
{
    private ICallerProvider? _callerProvider;

    public string Name { get; set; } = default!;

    public virtual bool UseDapr { get; set; } = false;

    protected IServiceProvider ServiceProvider { get; set; } = default!;

    protected ICallerProvider CallerProvider
    {
        get
        {
            if (_callerProvider is null)
            {
                _callerProvider = ServiceProvider.GetRequiredService<ICallerFactory>().CreateClient(Name);
            }
            return _callerProvider;
        }
    }

    public ServiceCaller(IServiceProvider _serviceProvider)
    {
        ServiceProvider = _serviceProvider;
    }
}

