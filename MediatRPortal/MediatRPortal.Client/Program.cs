using MediatR;
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
// No need to register the open generic type INotificationHandler<> as MediatR does this for us

builder.Services.AddScoped<DesignerService>();
builder.Services.AddScoped<RouteService>();

await builder.Build().RunAsync();
