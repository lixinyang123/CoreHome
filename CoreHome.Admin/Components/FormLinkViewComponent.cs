using CoreHome.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreHome.Admin.Components
{
    public class FormLinkViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FormLink formLink)
        {
            formLink.Id = Guid.NewGuid().ToString().Substring(0, 6);
            return View(formLink);
        }
    }
}
