using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using admin.Models;
using Aiursoft.Pylon.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace admin.Controllers
{
    public class OverviewController : VerifyController
    {
        private IPusher<WebSocket> _pusher;
        private static byte[] _data;
        private const int _length = 1024 * 1024 * 1;

        public OverviewController(IMemoryCache _cache) : base(_cache) 
        { 
            _pusher = new WebSocketPusher();
        }

        private static byte[] GetData()
        {
            if (_data == null)
            {
                _data = new byte[_length];
                for (int i = 0; i < _length; i++)
                {
                    _data[i] = 1;
                }
            }
            return _data;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AiurNoCache]
        [AiurForceWebSocket]
        public async Task<IActionResult> Pushing()
        {
            await _pusher.Accept(HttpContext);
            for (int i = 0; i < 36000 && _pusher.Connected; i++)
            {
                try
                {
                    _pusher.SendMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff") + $"|{i + 1}").GetAwaiter();
                    await Task.Delay(100);
                }
                catch
                {
                    break;
                }
            }
            return null;
        }

        [AiurNoCache]
        public IActionResult Download()
        {
            HttpContext.Response.Headers.Add("Content-Length", _length.ToString());
            return new FileContentResult(GetData(), "application/octet-stream");
        }

        [AiurNoCache]
        public JsonResult GetPing()
        {
            return Json(new List<object>());
        }
    }
}