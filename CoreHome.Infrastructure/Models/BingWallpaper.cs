using System.Text.Json.Serialization;

namespace CoreHome.Infrastructure.Models
{
    public class BingWallpaper
    {
        [JsonPropertyName("images")]
        public Image[] Images { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}
