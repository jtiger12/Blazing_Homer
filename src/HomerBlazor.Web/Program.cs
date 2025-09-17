using HomerBlazor.Core.Interfaces;
using HomerBlazor.Core.Services;
using HomerBlazor.Web.Components;
using HomerBlazor.Web.Services;
using HomerBlazor.ServiceCards.Services;
using HomerBlazor.ServiceCards.Interfaces;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/homer-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

// Configure Homer services
builder.Services.Configure<ConfigurationOptions>(options =>
{
    options.ConfigPath = builder.Configuration.GetValue<string>("Homer:ConfigPath") ?? "config/config.yml";
});

builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

// Add service card registry
builder.Services.AddSingleton<IServiceCardRegistry, ServiceCardRegistry>();

// Add JavaScript interop service
builder.Services.AddScoped<IJSInteropService, JSInteropService>();

// Add SignalR for real-time updates
builder.Services.AddSignalR();

// Add HTTP client for service cards
builder.Services.AddHttpClient();

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Optionally enable HTTPS redirection (disabled by default for containers)
var enableHttpsRedirect = app.Configuration.GetValue<bool>("EnableHttpsRedirection");
if (enableHttpsRedirect)
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();
