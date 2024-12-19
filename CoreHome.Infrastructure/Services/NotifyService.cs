using CoreHome.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class NotifyService(PusherConfig config)
    {
        private readonly string api = "https://wxpusher.zjiecode.com/api/send/message";

        private readonly string token = config.Token;

        private readonly string uid = config.Uid;

        private readonly HttpClient httpClient = new();

        public void PushNotify(string title, string content, string url = "")
        {
            NotificationBody notification = new(token, uid, title, content, url);

            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json"
            );

            _ = httpClient.PostAsync(api, httpContent);
        }
    }
}
