using CoreHome.Infrastructure.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreHome.Infrastructure.Services
{
    public class BingWallpaperService
    {
        private string urlCache;
        private int lastDay;

        public async Task<string> GetUrl()
        {
            int nowDay = DateTime.Now.Day;
            if (nowDay != lastDay)
            {
                try
                {
                    string url = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
                    using HttpClient httpClient = new HttpClient();
                    string jsonStr = await httpClient.GetStringAsync(url);
                    BingWallpaper wallpaper = JsonSerializer.Deserialize<BingWallpaper>(jsonStr);

                    urlCache = "https://cn.bing.com/" + wallpaper.images[0].url;
                    lastDay = nowDay;
                    return urlCache;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return urlCache;
            }

        }
    }
}
