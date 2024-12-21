using CoreHome.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Components
{
    public class ConfirmLinkViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ConfirmLink confirmLink)
        {
            confirmLink.Id = Guid.NewGuid().ToString()[..6];
            return View(confirmLink);
        }
    }
}
