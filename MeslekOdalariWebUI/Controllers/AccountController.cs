using MeslekOdalari.Dto.Dtos.IdentityDtos;
using MeslekOdalari.Entity.Entities;
using MeslekOdalari.Entity.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;

namespace MeslekOdalariWebUI.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                TC = registerDto.TC,
                NameSurName = registerDto.NameSurname,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                UserRole = UserRoles.Esnaf,  // Burada rolü Esnaf yapıyoruz
                IsApproved = false,                // Başlangıçta onay false olabilir
                RegistrationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.TC == loginDto.TC);

            if (user == null)
            {
                ModelState.AddModelError("", "TC veya Şifre hatalı");
                return View();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "TC veya Şifre hatalı");
                return View();
            }
            // MongoDB ile uyumlu giriş yöntemi
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserRole", ((int)user.UserRole).ToString()),
                new Claim("TC", user.TC),
                new Claim("UserId", user.Id.ToString()) // Ekstra ID claim'i
            };

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Identity.Application", claimsPrincipal);

            // Kullanıcı rolüne göre yönlendirme
            if (user.UserRole == UserRoles.Admin)
            {
                return RedirectToAction("Index", "Banner");
            }
            else // Esnaf veya diğer roller
            {
                return RedirectToAction("Index", "Default");
            }
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ObjectId.TryParse(userIdString, out ObjectId userId))
            {
                return RedirectToAction("Login");
            }

            // Doğrudan LINQ ile kullanıcıyı bul
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var profileDto = new ProfileDto
            {
                TC = user.TC,
                NameSurName = user.NameSurName,
                Email = user.Email,
                UserName = user.UserName,
                RegistrationDate = user.RegistrationDate
            };
            return View(profileDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return View(profileDto);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ObjectId.TryParse(userIdString, out ObjectId userId))
            {
                return RedirectToAction("Login");
            }

            // Doğrudan LINQ ile kullanıcıyı bul
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            user.NameSurName = profileDto.NameSurName;
            user.Email = profileDto.Email;
            user.UserName = profileDto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.Message = "Profil bilgileri başarıyla güncellendi.";
                ViewBag.MessageType = "success";
            }
            else
            {
                ViewBag.Message = "Profil güncellenirken bir hata oluştu.";
                ViewBag.MessageType = "error";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(profileDto);
        }
        public async Task<IActionResult> Logout()
        {
            // Manuel oluşturduğun cookie'yi de temizle
            await HttpContext.SignOutAsync("Identity.Application");

            // Identity ile giriş yapıldıysa onu da temizle
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Default");
        }

    }
}
//accountcontroller son hali çalışıyo esnaf atıyor.