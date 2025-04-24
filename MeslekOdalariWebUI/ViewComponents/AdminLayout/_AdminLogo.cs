using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.AdminLayout
{
    public class _AdminLogo:ViewComponent
    {
       public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
