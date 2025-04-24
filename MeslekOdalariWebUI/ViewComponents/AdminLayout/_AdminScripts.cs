using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.AdminLayout
{
    public class _AdminScripts:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
