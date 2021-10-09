var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<Program>();
    })
    .AddTransient(typeof(IMiddleware<>), typeof(ValidatorMiddleware<>))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA EShop - Payment HTTP API",
            Version = "v1",
            Description = "The Payment Service HTTP API"
        });
    })
    .AddDomainEventBus(options =>
    {
        options.UseEventBus(Assembly.GetEntryAssembly()!,
                            typeof(OrderPaymentFailedIntegrationEvent).Assembly)
               .UseUoW<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
               .UseDaprEventBus<IntegrationEventLogService>()
               .UseEventLog<PaymentDbContext>()
               .UseRepository<PaymentDbContext>()
               ;
    })
    .AddServices(builder);

app.MigrateDbContext<PaymentDbContext>((context, services) =>
{
});

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA EShop Service HTTP API v1");
});

app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoint =>
{
    endpoint.MapSubscribeHandler();
});
app.Run();

public partial class Program
{
    public static string Namespace = typeof(IntegrationEventService).Namespace!;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}
