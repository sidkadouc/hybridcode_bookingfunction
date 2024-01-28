using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BookingFlightsPocAPI.Services;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient("TravelAPI", client =>
        {
            client.BaseAddress = new Uri("https://test.api.amadeus.com");
        });
        services.AddScoped<TravelAPI>();
    })
    .Build();

host.Run();
