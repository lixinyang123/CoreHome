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
        /// <param name="SiteMapPath">站点地图文件路径</param>
        public async void PushToBaidu(string WebRootPath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                string SiteMapPath = WebRootPath + "\\SiteMap.txt";
                string linkStr = File.ReadAllText(SiteMapPath);
                HttpContent content = new StringContent(linkStr, Encoding.UTF8);

                await httpClient.PostAsync(baiduLinkSubmit, content);
            }
            catch (Exception) { }
        }
    }
}
