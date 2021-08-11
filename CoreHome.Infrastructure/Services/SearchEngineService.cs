using System.Text;

namespace CoreHome.Infrastructure.Services
{
    public class SearchEngineService
    {
        private readonly string baiduLinkSubmit;

        public SearchEngineService(string baiduLinkSubmit)
        {
            this.baiduLinkSubmit = baiduLinkSubmit;
        }

        /// <summary>
        /// PushLinkToBaidu
        /// </summary>
        /// <param name="WebRootPath">站点地图的存放路径</param>
        public async Task<string> PushToBaidu(string WebRootPath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                string SiteMapPath = Path.Combine(WebRootPath, "SiteMap.txt");
                string linkStr = File.ReadAllText(SiteMapPath);
                HttpContent content = new StringContent(linkStr, Encoding.UTF8);

                HttpResponseMessage responseMessage = await httpClient.PostAsync(baiduLinkSubmit, content);
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
