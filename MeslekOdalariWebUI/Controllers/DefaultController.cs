using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MeslekOdalari.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System.Security.Claims;

public class DefaultController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public DefaultController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            // UserManager.GetUserAsync yerine manual bulma
            var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && ObjectId.TryParse(userIdClaim.Value, out ObjectId userId))
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    ViewBag.UserRole = (int)user.UserRole;
                    ViewBag.UserName = user.NameSurName;
                }
            }
        }

        return View();
    }
}
