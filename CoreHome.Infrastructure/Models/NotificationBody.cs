using System.Text.Json.Serialization;

namespace CoreHome.Infrastructure.Models
{
    public class NotificationBody
    {
        public NotificationBody(string token, string uid, string summery, string content, string url = "")
        {
            AppToken = token;
            Summary = summery;
            Content = content;
            Uids = new() { uid };
            Summary = "CoreHome Notification";
            ContentType = 3;
            VerifyPay = false;
            Url = url;
        }

        [JsonPropertyName("appToken")]
        public string AppToken { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("contentType")]
        public int ContentType { get; set; }

        [JsonPropertyName("uids")]
        public List<string> Uids { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("verifyPay")]
        public bool VerifyPay { get; set; }
    }
}
