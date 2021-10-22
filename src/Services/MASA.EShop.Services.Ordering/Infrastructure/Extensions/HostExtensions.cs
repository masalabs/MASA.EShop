namespace MASA.EShop.Services.Ordering.Infrastructure.Extensions;

public static class HostExtensions
{
    public static void MigrateDbContext<TContext>(this IHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using (var scope = webHost.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TContext>();
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}

