using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nexus.merge;
using nexus.merge.Pocos;

class Program
{
  static async Task Main(string[] args)
  {
    var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
      .ConfigureAppConfiguration((hostingContext, config) =>
      {
        config.AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .Build();
      })
      .ConfigureServices((hostContext, services) =>
      {
        services.AddSingleton<AdoApiHelper>();

        services.Configure<AdoSettings>(hostContext.Configuration.GetSection("Ado"));

        services.AddTransient<App>();
        services.AddTransient<MergeHelper>();
      });


    var host = builder.Build();

    using (var scope = host.Services.CreateScope())
    {
      var services = scope.ServiceProvider;

      var app = services.GetRequiredService<App>();

      await app.Run();
    }
  }
}
