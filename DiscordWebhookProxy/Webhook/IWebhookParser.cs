namespace DiscordWebhookProxy.Webhook
{
    public interface IWebhookParser
    {
        string Parse(string body);
    }
}
