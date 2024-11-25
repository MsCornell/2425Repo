using Game;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//player
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
//game
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/Game";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Game";
    }

    return new Logic.GameRepository(url);
});

//character
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/Character";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Character";
    }

    return new Logic.CharacterRepository(url);
});


// gameboard
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/Game_Board";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Game_Board";
    }

    return new Logic.GameBoardRepository(url);
});

//board
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/Board";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Board";
    }

    return new Logic.BoardRepository(url);
});

//PlayerWinRate
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/PlayerWinRate";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/PalyerWinRate";
    }

    return new Logic.PlayerWinRateRepository(url);
});
//GameBoardDetail
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/GameBoardDetail";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/GameBoardDetail";
    }

    return new Logic.GameBoardDetailRepository(url);
});
//GameDetail
builder.Services.AddScoped(serviceProvider =>
{
    string url;

    if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/GameDetail";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/GameDetail";
    }

    return new Logic.GameDetailRepository(url);
});


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
