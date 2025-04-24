using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.AdminLayout
{
    public class _AdminSidebar:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
