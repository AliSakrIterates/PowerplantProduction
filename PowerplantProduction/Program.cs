using Microsoft.AspNetCore.Hosting;

namespace PowerplantProduction.Hosting.Controllers;
public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        CreateDbIfNotExists(host);

        host.Run();
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using var scope = host.Services.CreateScope();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
