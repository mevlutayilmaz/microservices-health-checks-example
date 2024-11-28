using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecksUI(
    settings =>
    {
        settings.AddHealthCheckEndpoint("Service A", "https://localhost:7214/health")
                .AddHealthCheckEndpoint("Service B", "https://localhost:7018/health")
                .SetEvaluationTimeInSeconds(3)
                .SetApiMaxActiveRequests(3);

        settings.ConfigureApiEndpointHttpclient((serviceProvider, httpClient) =>
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...."));
        settings.ConfigureWebhooksEndpointHttpclient((serviceProvider, httpClient) =>
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...."));
    }
).AddSqlServerStorage("Server=localhost, 1433;Database=HealthCheckUIDB;User ID=SA;Password=1q2w3e4r+!;TrustServerCertificate=True");

var app = builder.Build();

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.Run();