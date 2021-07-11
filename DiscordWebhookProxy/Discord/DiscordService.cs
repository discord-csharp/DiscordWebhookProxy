using Microsoft.Extensions.Caching.Memory;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordWebhookProxy.Discord
{

    public record HackedWebhook
    (
        Snowflake ID,
        WebhookType Type,
        Optional<Snowflake?> GuildID,
        Snowflake? ChannelID,
        Optional<IUser> User,
        string Name,
        string Avatar,
        Optional<string> Token = default,
        Snowflake? ApplicationID = default,
        Optional<IPartialGuild> SourceGuild = default,
        Optional<IPartialChannel> SourceChannel = default,
        Optional<string> URL = default
    );

    public class DiscordService : IDiscordService
    {
        private readonly IMemoryCache _cache;
        private readonly IDiscordRestUserAPI _userApi;
        private readonly IDiscordRestWebhookAPI _webhookApi;
        private readonly DiscordHttpClient _discordHttpClient;
        private readonly HttpClient _httpClient;

        public DiscordService(IDiscordRestUserAPI userApi, IDiscordRestWebhookAPI webhookApi, DiscordHttpClient discordHttpClient, IHttpClientFactory httpFactory, IMemoryCache cache)
        {
            _cache = cache;
            _userApi = userApi;
            _webhookApi = webhookApi;
            _discordHttpClient = discordHttpClient;
            _httpClient = httpFactory.CreateClient(nameof(DiscordService));
        }

        public async ValueTask<IReadOnlyList<ulong>> GetBotGuildsAsync()
        {
            var guilds = await _cache.GetOrCreateAsync("guilds", async entry =>
            {
                entry.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));
                var guilds = await _userApi.GetCurrentUserGuildsAsync();

                return guilds.Entity.Select(a => a.ID.Value.Value);
            });

            return guilds.ToList().AsReadOnly();
        }

        public async ValueTask<ulong> GetWebhookGuildIdAsync(ulong webhookId)
        {
            var guildId = await _cache.GetOrCreateAsync(webhookId, async entry =>
            {
                var webhook = await _discordHttpClient.GetAsync<HackedWebhook>($"/api/v9/webhooks/{webhookId}");
                return webhook.Entity.GuildID.Value.Value.Value;
            });

            

            return guildId;
        }

        public async ValueTask<WebhookResponse> SendWebhook(ulong id, string token, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{id}/{token}", content);

            if(!response.IsSuccessStatusCode)
            {
                return new WebhookResponse((int)response.StatusCode, string.Empty);
            }
            var result = new WebhookResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            return result;
        }
    }
}
