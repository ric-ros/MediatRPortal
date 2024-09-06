using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Pages;
using MediatRPortal.Client.Services;
using MediatRPortal.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MediatRPortal.Client._Imports).Assembly);
});
//builder.Services.AddTransient(typeof(INotificationHandler<>), typeof(NotificationComponentBase<>));
// No need to register the open generic type INotificationHandler<> as MediatR does this for us

builder.Services.AddScoped<DesignerService>();
builder.Services.AddScoped<RouteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MediatRPortal.Client._Imports).Assembly);

app.Run();