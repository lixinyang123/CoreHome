using System.Net.Http;

namespace Infrastructure.Service
{
    public static class NotifyManager
    {
        private static readonly string sckey = "SCU53487T3f2525ad756287352c78dbff72f9f6525d0463b66ac44";

        public static void PushNotify(string text, string desp)
        {
            string url = $" https://sc.ftqq.com/{sckey}.send?text={text}&desp={desp}";

            new HttpClient().GetAsync(url);
        }
    }
}
