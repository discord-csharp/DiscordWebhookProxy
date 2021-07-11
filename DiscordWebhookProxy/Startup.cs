using DiscordWebhookProxy.Discord;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Remora.Discord.Rest.Extensions;
using DiscordWebhookProxy.Webhook;

namespace DiscordWebhookProxy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient(nameof(DiscordService), client => { client.BaseAddress = new Uri("https://discordapp.com/api/webhooks/"); });
            services.AddDiscordRest(services => Configuration["DiscordToken"]);
            
            services.AddSingleton<IDictionary<string, IWebhookParser>>(provider => {
                return new Dictionary<string, IWebhookParser>
                {
                    ["github"] = ActivatorUtilities.CreateInstance<GithubWebhookParser>(provider)
                };
            });
            services.AddScoped<IDiscordService, DiscordService>();
            services.AddMemoryCache(o => { o.ExpirationScanFrequency = TimeSpan.FromMinutes(1); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
