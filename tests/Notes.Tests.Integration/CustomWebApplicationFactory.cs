using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Notes.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<API.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.Test.json");
            config.AddJsonFile(path, optional: true);
        });
    }
}