using System.IO;
using System.Net.Http;
using System.Text;

namespace CoreHome.Infrastructure.Services
{
    public class SearchEngineService
    {
        /// <summary>
        /// PushLinkToBaidu
        /// </summary>
        /// <param name="SiteMapPath">站点地图文件路径</param>
        public async void PushToBaidu(string WebRootPath)
        {
            HttpClient httpClient = new HttpClient();
            string url = @"http://data.zz.baidu.com/urls?site=https://www.lllxy.net&token=5ZG2x3hnCmpkN4Qh";

            string SiteMapPath = WebRootPath + "\\SiteMap.txt";
            string linkStr = File.ReadAllText(SiteMapPath);
            HttpContent content = new StringContent(linkStr, Encoding.UTF8);

            await httpClient.PostAsync(url, content);
        }
    }
}
