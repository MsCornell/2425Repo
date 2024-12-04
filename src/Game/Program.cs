using Game;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<Game.Services.GameStateService>();
builder.Services.AddSingleton<Game.Services.PlayerStateService>();


string url;

if (builder.HostEnvironment.IsDevelopment())
    {
        url = "http://localhost:5000/api/";
    }
    else
    {
        url = "https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/";
    }

//player
builder.Services.AddScoped(x => new Logic.PlayerRepository(url + "Player"));


//game
builder.Services.AddScoped(x => new Logic.GameRepository(url + "Game"));

//character
builder.Services.AddScoped(x => new Logic.CharacterRepository(url + "Character"));


// gameboard
builder.Services.AddScoped(x => new Logic.GameBoardRepository(url + "Game_Board"));


//board
builder.Services.AddScoped(x => new Logic.BoardRepository(url + "Board"));


//PlayerWinRate
builder.Services.AddScoped(x => new Logic.PlayerWinRateRepository(url + "PlayerWinRate"));


//GameBoardDetail
builder.Services.AddScoped(x => new Logic.GameBoardDetailRepository(url + "GameBoardDetail"));


//GameDetail
builder.Services.AddScoped(x => new Logic.GameDetailRepository(url + "GameDetail"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
