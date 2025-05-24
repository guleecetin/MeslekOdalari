using MeslekOdalari.Dto.Dtos.IdentityDtos;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                NameSurName = registerDto.NameSurname,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
              
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View();
            }
            return RedirectToAction("Login");
        }
    }
}
