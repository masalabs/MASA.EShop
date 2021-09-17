
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLazyWebApplication(builder)
    .AddServices();

var app = builder.Services.BuildServiceProvider().GetRequiredService<WebApplication>();

app.Run();
