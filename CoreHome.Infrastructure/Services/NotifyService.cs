namespace CoreHome.Infrastructure.Services
{
    public class NotifyService
    {
        private readonly string url = "https://api2.pushdeer.com/message/push";

        private readonly string sckey;

        public NotifyService(string sckey)
        {
            this.sckey = sckey;
        }

        public void PushNotify(string text, string desp)
        {
            HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("pushkey", sckey),
                new KeyValuePair<string, string>("text", $"{text}\n\n\n{desp}")
            });

            new HttpClient().PostAsync(url, content);
        }
    }
}
