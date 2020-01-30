using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreHome.Admin.Models
{
    public interface IPusher<T>
    {
        bool Connected { get; }
        Task Accept(HttpContext context);
        Task SendMessage(string message);
    }
}
