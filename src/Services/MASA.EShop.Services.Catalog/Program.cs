
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

builder.Services.AddServices();

app.MapGet("/", () => "Hello World!");

app.Run();
