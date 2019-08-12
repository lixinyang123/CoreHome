using System;
using System.Net.Http;
using System.Text;
using System.IO;

namespace coreHome.Servrice
{
    public static class SearchEngineService
    {
        /// <summary>
        /// PushLinkToBaidu
        /// </summary>
        /// <param name="SiteMapPath">站点地图文件路径</param>
        public static async void PushToBaidu(string WebRootPath)
        {
            HttpClient httpClient = new HttpClient();
            string url = @" http://data.zz.baidu.com/urls?site=https://www.lllxy.site&token=5ZG2x3hnCmpkN4Qh";

            string SiteMapPath = WebRootPath + "\\SiteMap.txt";
            string linkStr = File.ReadAllText(SiteMapPath);
            HttpContent content = new StringContent(linkStr, Encoding.UTF8);

            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
            string responseStr = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine(responseStr);
        }
    }
}
