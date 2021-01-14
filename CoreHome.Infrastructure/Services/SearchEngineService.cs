using System;
using System.IO;
using System.Net.Http;
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
        public async void PushToBaidu(string WebRootPath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                string SiteMapPath = WebRootPath + "\\SiteMap.txt";
                string linkStr = File.ReadAllText(SiteMapPath);
                HttpContent content = new StringContent(linkStr, Encoding.UTF8);

                HttpResponseMessage responseMessage = await httpClient.PostAsync(baiduLinkSubmit, content);
                string result = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            catch (Exception) { }
        }
    }
}
