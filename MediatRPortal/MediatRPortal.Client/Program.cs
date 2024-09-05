using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
//builder.Services.AddScoped(typeof(INotificationHandler<>), typeof(NotificationComponentBase<>));
//builder.Services.AddTransient<INotificationHandler<INotification>, NotificationComponentBase>();

builder.Services.AddScoped<DesignerService>();
builder.Services.AddScoped<RouteService>();

await builder.Build().RunAsync();
