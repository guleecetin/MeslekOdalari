using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class AhilikController : Controller
    {
        public IActionResult AhilikKulturu()
        {
            return View();
        }

        public IActionResult AhilikNedir()
        {
            return View();
        }
    }
}
