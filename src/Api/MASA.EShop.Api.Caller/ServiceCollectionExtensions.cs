namespace MASA.EShop.Api.Caller;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCallerService(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly = assembly ?? Assembly.GetCallingAssembly();
        services.AddCaller(options =>
        {
            var httpCallerTypes = assembly.GetTypes()
                        .Where(a => a.IsAssignableTo(typeof(HttpClientCaller)));
            foreach (var httpCallerType in httpCallerTypes)
            {
                services.AddScoped(httpCallerType);
                var serviceCaller = (HttpClientCaller)services.BuildServiceProvider().GetRequiredService(httpCallerType);
                options.UseHttpClient(opt =>
                {
                    opt.Name = serviceCaller.Name;
                    opt.Configure = (client) =>
                    {
                        client.BaseAddress = new Uri(serviceCaller.BaseAddress);
                    };
                })
                .AddHttpMessageHandler(() => serviceCaller.CreateHttpMessageHandler());
            }

            var daprCallerTypes = assembly.GetTypes()
                        .Where(a => a.IsAssignableTo(typeof(DaprClientCaller)));
            foreach (var daprCallerType in daprCallerTypes)
            {
                services.AddScoped(daprCallerType);
                var serviceCaller = (DaprClientCaller)services.BuildServiceProvider().GetRequiredService(daprCallerType);
                options.UseDapr(opt =>
                {
                    opt.Name = serviceCaller.Name;
                    opt.AppId = serviceCaller.AppId;
                });
            }
        });

        return services;
    }
}
