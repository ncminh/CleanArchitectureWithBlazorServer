using Blazor.Server.UI;
using Blazor.Server.UI.Services.Notifications;
using CleanArchitecture.Blazor.Application;
using CleanArchitecture.Blazor.Infrastructure;
using CleanArchitecture.Blazor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterSerilog();
builder.Services.AddBlazorUiServices();
builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationServices();

var app = builder.Build();

app.MapHealthChecks("/health");
app.UseExceptionHandler("/Error");
app.MapFallbackToPage("/_Host");
app.UseInfrastructure(builder.Configuration);
app.UseWebSockets();
app.MapBlazorHub(options=>options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);

if (app.Environment.IsDevelopment())
{
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
        var notificationService = scope.ServiceProvider.GetService<INotificationService>();
        if (notificationService is InMemoryNotificationService inMemoryNotificationService)
        {
            inMemoryNotificationService.Preload();
        }
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
await app.RunAsync();