namespace Masa.EShop.Services.Catalog.Infrastructure.Extensions;

public static class HostExtensions
{
    public static void MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TContext>();
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
