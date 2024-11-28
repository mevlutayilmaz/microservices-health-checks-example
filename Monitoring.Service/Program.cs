using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecksUI(
    settings =>
    {
        settings.AddHealthCheckEndpoint("Service A", "https://localhost:7038/health")
                .AddHealthCheckEndpoint("Service B", "https://localhost:7176/health")
                .SetEvaluationTimeInSeconds(3)
                .SetApiMaxActiveRequests(3);

        settings.ConfigureApiEndpointHttpclient((serviceProvider, httpClient) =>
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...."));
        settings.ConfigureWebhooksEndpointHttpclient((serviceProvider, httpClient) =>
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...."));
    }
).AddSqlServerStorage("Server=.;Database=HealthCheckUIDB;Integrated Security=True;TrustServerCertificate=True;");

var app = builder.Build();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.Run();