using System.Net.Http;

namespace CoreHome.Infrastructure.Services
{
    public class NotifyService
    {
        public string Sckey { get; set; }

        public NotifyService(string sckey)
        {
            Sckey = sckey;
        }

        public void PushNotify(string text, string desp)
        {
            string url = $" https://sc.ftqq.com/{Sckey}.send?text={text}&desp={desp}";

            new HttpClient().GetAsync(url);
        }
    }
}
