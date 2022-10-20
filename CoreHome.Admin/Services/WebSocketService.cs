using System.Net.WebSockets;
using System.Text;

namespace CoreHome.Admin.Services
{
    public static class WebSocketService
    {
        public static async Task SendMessage(this WebSocket ws, string message)
        {
            ArraySegment<byte> buffer = new(Encoding.UTF8.GetBytes(message));
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
