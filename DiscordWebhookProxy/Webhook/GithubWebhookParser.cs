
using DiscordWebhookProxy.Discord;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DiscordWebhookProxy.Webhook
{
    public class GithubWebhookParser : IWebhookParser
    {
        private readonly JsonSerializerOptions _jsonSerializerOpts;
        public IDictionary<string, Func<JsonDocument, Embed>> _handlers;

        public GithubWebhookParser(IOptions<JsonSerializerOptions> jsonSerializerOpts)
        {
            _jsonSerializerOpts = jsonSerializerOpts.Value;
            _handlers = new Dictionary<string, Func<JsonDocument, Embed>>
            {
                ["published"] = HandlePublished
            }; 
        }

        public string Parse(string rawBody)
        {
            var body = JsonDocument.Parse(rawBody);

            var action = body.RootElement.GetProperty("action").GetString();
            Embed embed;
            if(_handlers.TryGetValue(action, out var handler))
            {
                embed = handler(body);
            }
            else 
            {
                embed = new Embed("Unknown Github Webhook Event")
                {
                    Description = $"**Action type:** {action}\n**First 1000 characters of the event:** {rawBody[0..999]}",
                };
            }
            var avatar = "https://cdn.discordapp.com/avatars/789924237925417020/df91181b3f1cf0ef1592fbe18e0962d7.webp?size=256";
            var result = new DiscordWebhookExecute(string.Empty, "GitHub", avatar, new[] { embed });

            var json = JsonSerializer.Serialize(result, _jsonSerializerOpts);

            return json;
        }

        public Embed HandlePublished(JsonDocument document)
        {
            var repository = document.RootElement.GetProperty("repository");
            var repoName = repository.GetProperty("full_name").GetString();
            var avatarUrl = repository.GetProperty("owner").GetProperty("avatar_url").GetString();
            var registryPackage = document.RootElement.GetProperty("registry_package");
            var htmlUrl = registryPackage.GetProperty("html_url").GetString();
            var packageVersion = registryPackage.GetProperty("package_version");
            var containerMetadata = packageVersion.GetProperty("container_metadata");
            var tagName = containerMetadata.GetProperty("tag").GetProperty("name").GetString();
            var packageUrl = packageVersion.GetProperty("package_url").GetString();

            var embed = new Embed
            {
                Author = new EmbedAuthor(repoName, htmlUrl, avatarUrl),
                Description = $"New container pushed to {repoName} packages",
                Fields = new[] { new EmbedField("Container Image", $"[{packageUrl}]({htmlUrl})") },
                Title = packageUrl,
                Type = EmbedType.Rich,
            };

            return embed;
        }
    }
}
