using Remora.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordWebhookProxy.Discord
{
    public interface IDiscordService
    {
        ValueTask<IReadOnlyList<ulong>> GetBotGuildsAsync();
        ValueTask<ulong> GetWebhookGuildIdAsync(ulong webhookId);
        ValueTask<WebhookResponse> SendWebhook(ulong id, string token, string body);
    }
}
