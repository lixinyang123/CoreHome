using CoreHome.Infrastructure.Models;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class BingWallpaperService()
    {
        private const string API = "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
        private readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(3) };

        private int lastDate;
        private string urlCache;

        public string GetUrl()
        {
            int date = DateTime.Now.Day;
            if (date == lastDate) return urlCache;

            try
            {
                string jsonStr = httpClient.GetStringAsync(API).Result;
                BingWallpaper wallpaper = JsonSerializer.Deserialize<BingWallpaper>(jsonStr);
                urlCache = $"https://www.bing.com/{wallpaper.Images.First().Url}";
            }
            catch (Exception)
            {
                return string.Empty;
            }

            lastDate = date;
            return urlCache;
        }
    }
}
