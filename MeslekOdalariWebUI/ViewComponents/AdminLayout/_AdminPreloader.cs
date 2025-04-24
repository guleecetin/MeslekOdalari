using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.AdminLayout
{
    public class _AdminPreloader:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
