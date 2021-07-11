using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DiscordWebhookProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureLogging((context, builder) =>
                    {
                        builder.ClearProviders();
                        if(context.HostingEnvironment.IsDevelopment()){
                            builder.AddDebug();
                        }

                        builder.AddSimpleConsole(o =>
                        {
                            o.ColorBehavior = LoggerColorBehavior.Disabled;
                            o.SingleLine = true;
                            o.TimestampFormat = "o";
                        });

                        builder.AddFilter("System.Net.Http.HttpClient.*.LogicalHandler", LogLevel.Warning)
                        .AddFilter("System.Net.Http.HttpClient.*.ClientHandler", LogLevel.Warning);
                    })
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddKeyPerFile("/var/secrets", optional: true);
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            builder.AddUserSecrets("17f430af-18cb-4b6f-89fe-8694b3e8ed6a");
                        }
                    });
                });
    }
}
