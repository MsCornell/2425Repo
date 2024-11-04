using Game;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/Player";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Player";
    }

    return new Logic.PlayerRepository(url);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
