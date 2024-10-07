using Client.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Named HttpClient for API_Prefix
builder.Services.AddHttpClient("FunctionClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["API_Prefix"]
        ?? throw new Exception("Missing ENV VAR API_Prefix for Function, check that /Client/appsettings.Development.json reflects /Api/Properties/launchSettings.json"));
});

// Named HttpClient for DATAAPI_Prefix
builder.Services.AddHttpClient("DatabaseClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DATAAPI_Prefix"]
        ?? throw new Exception("Missing ENV VAR DATAAPI_Prefix for Function, check that /Client/appsettings.Development.json reflects /Api/Properties/launchSettings.json"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
