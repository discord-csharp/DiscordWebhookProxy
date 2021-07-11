using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DiscordWebhookProxy.Webhook;
using DiscordWebhookProxy.Discord;
using Microsoft.Extensions.Caching.Memory;

namespace DiscordWebhookProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class WebhookProxyController : ControllerBase
    {
        private readonly IDiscordService _discordService;
        private readonly IDictionary<string, IWebhookParser> _parsers;
        private readonly IMemoryCache _cache;

        public WebhookProxyController(IDictionary<string, IWebhookParser> webhookParsers, IDiscordService discordService, IMemoryCache cache)
        {
            _discordService = discordService;
            _parsers = webhookParsers;
            _cache = cache;
        }

        [HttpPost("{id}/{token}/{format}")]
        [HttpPost("{id}/{token}")]
        public async Task<IActionResult> PostAsync(ulong id, string token, string format = "")
        {
            if (!_parsers.ContainsKey(format))
            {
                return BadRequest();
            }

            var cacheKey = $"invalid-webhooks-{id}";

            if (_cache.TryGetValue(cacheKey, out int invalidEntryCount) && invalidEntryCount > 5)
            {
                return BadRequest();
            }

            var botGuilds = await _discordService.GetBotGuildsAsync();
            var webhookGuild = await _discordService.GetWebhookGuildIdAsync(id);
            if (!botGuilds.Contains(webhookGuild))
            {
                _cache.Set(cacheKey, invalidEntryCount + 1, DateTimeOffset.UtcNow.AddHours(1));
                return NotFound();
            }

            using var sr = new StreamReader(HttpContext.Request.Body);
            var body = await sr.ReadToEndAsync();

            var discordBody = _parsers[format].Parse(body);
            var response = await _discordService.SendWebhook(id, token, discordBody);

            if (response.StatusCode > 299)
            {
                _cache.Set(cacheKey, invalidEntryCount + 1, DateTimeOffset.UtcNow.AddHours(1));
            }

            if (invalidEntryCount > 0)
            {
                _cache.Remove(cacheKey);
            }
            
            return new StatusCodeResult(response.StatusCode);
        }
    }
}
