using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost",
                policy =>
                {
                    policy.WithOrigins("http://localhost:1234", "http://localhost:2345")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    })
    .Build();

// Do you have the local tools installed?
// https://github.com/Azure/azure-functions-core-tools#installing

host.Run();
