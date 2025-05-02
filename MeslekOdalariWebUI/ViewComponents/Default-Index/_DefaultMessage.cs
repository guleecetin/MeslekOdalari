using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.ViewComponents.Default_Index
{
    public class _DefaultMessage:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
