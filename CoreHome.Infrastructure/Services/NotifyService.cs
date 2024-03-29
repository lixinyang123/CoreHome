﻿using CoreHome.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace CoreHome.Infrastructure.Services
{
    public class NotifyService
    {
        private readonly string api = "https://wxpusher.zjiecode.com/api/send/message";

        private readonly string token;

        private readonly string uid;

        private readonly HttpClient httpClient;

        public NotifyService(PusherConfig config)
        {
            token = config.Token;
            uid = config.Uid;
            httpClient = new();
        }

        public void PushNotify(string title, string content, string url = "")
        {
            NotificationBody notification = new(token, uid, title, content, url);

            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json"
            );

            _ = httpClient.PostAsync(api, httpContent);
        }
    }
}
