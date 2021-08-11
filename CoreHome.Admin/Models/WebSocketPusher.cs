using CoreHome.Admin.Services;
using System.Net.WebSockets;

namespace CoreHome.Admin.Models
{
    public class WebSocketPusher : IPusher<WebSocket>
    {
        private WebSocket _ws;
        public bool Connected => _ws.State == WebSocketState.Open;

        public async Task Accept(HttpContext context)
        {
            _ws = await context.WebSockets.AcceptWebSocketAsync();
        }

        public async Task SendMessage(string message)
        {
            await _ws.SendMessage(message);
        }
    }
}
