namespace MASA.EShop.Api.Caller;

public class HttpClientCaller : ServiceCaller
{
    public string BaseAPI { get; set; } = default!;

    public string BaseAddress { get; set; } = default!;

    public HttpClientCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }

    public virtual DelegatingHandler CreateHttpMessageHandler()
    {
        return new DefaultDelegatingHandler();
    }
}

class DefaultDelegatingHandler : DelegatingHandler
{

}

