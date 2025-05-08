using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class UyeController : Controller
    {
       // Üye kayıt bilgi sayfasını gösteren action method
        public IActionResult KayitBilgileri()
        {
            return View();
        }
    }
}
