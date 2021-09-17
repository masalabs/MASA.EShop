
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLazyWebApplication(builder)
    .AddServices();

var app = builder.Services.BuildServiceProvider().GetRequiredService<WebApplication>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

app.Run();
