namespace CoreHome.Infrastructure.Services
{
    public class NotifyService
    {
        private readonly string url = "https://api2.pushdeer.com/message/push";

        private readonly string sckey;

        private readonly HttpClient httpClient;

        public NotifyService(string sckey)
        {
            this.sckey = sckey;
            httpClient = new();
        }

        public void PushNotify(string text, string content)
        {
            HttpContent httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("pushkey", sckey),
                new KeyValuePair<string, string>("text", $"{text}\n\n\n{content}")
            });

            httpClient.PostAsync(url, httpContent);
        }
    }
}
