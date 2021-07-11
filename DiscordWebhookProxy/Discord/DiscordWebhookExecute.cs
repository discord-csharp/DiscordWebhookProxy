using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using System.Collections.Generic;

namespace DiscordWebhookProxy.Discord
{
    public record DiscordWebhookExecute(string Content, string Username, string AvatarUrl, IReadOnlyList<IEmbed> Embeds);
}
