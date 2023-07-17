using System.Text;
using System.Text.Json;
using CoreHome.Infrastructure.Models;

namespace CoreHome.Infrastructure.Services
{
    public class NotifyService
    {
        private readonly string url = "https://wxpusher.zjiecode.com/api/send/message";

        private readonly string token;

        private readonly string uid;

        private readonly HttpClient httpClient;

        public NotifyService(PushDeerConfig config)
        {
            this.token = config.Token;
            this.uid = config.Uid;
            httpClient = new();
        }

        public async void PushNotify(string text, string content)
        {
            NotificationBody notification = new(token,  uid, text, content);

            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(notification), 
                Encoding.UTF8, 
                "application/json"
            );

            HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            string result = await response.Content.ReadAsStringAsync();
        }
    }
}
