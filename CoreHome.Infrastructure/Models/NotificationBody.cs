using System.Text.Json.Serialization;

namespace CoreHome.Infrastructure.Models
{
    public class NotificationBody
    {
        public NotificationBody(string token, string uid, string summery, string content)
        {
            AppToken = token;
            Summary = summery;
            Content = content;
            Uids = new() { uid };
            Summary = "CoreHome 通知";
            ContentType = 3;
            VerifyPay = false;
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

        [JsonPropertyName("verifyPay")]
        public bool VerifyPay { get; set; }
    }
}
