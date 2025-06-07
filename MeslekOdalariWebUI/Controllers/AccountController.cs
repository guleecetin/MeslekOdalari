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
using MeslekOdalariWebUI.Models.Services;

namespace MeslekOdalariWebUI.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

            // Kayıt başarılıysa hoş geldiniz e-postası gönder
            try
            {
                string subject = "Kuaför ve Berberler Odası - Hoş Geldiniz!";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #2c3e50; text-align: center;'>Hoş Geldiniz!</h2>
                        <p>Sayın <strong>{user.NameSurName}</strong>,</p>
                        <p>Kuaför ve Berberler Odası'na başarıyla kayıt oldunuz. Hesabınız oluşturulmuştur.</p>
                        
                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <h4 style='color: #495057; margin-top: 0;'>Hesap Bilgileriniz:</h4>
                            <p><strong>Ad Soyad:</strong> {user.NameSurName}</p>
                            <p><strong>E-posta:</strong> {user.Email}</p>
                            <p><strong>Kullanıcı Adı:</strong> {user.UserName}</p>
                            <p><strong>Kayıt Tarihi:</strong> {user.RegistrationDate:dd.MM.yyyy HH:mm}</p>
                        </div>
                        
                        <p>Artık sistemimizi kullanmaya başlayabilirsiniz. Giriş yapmak için TC Kimlik numaranız ve şifrenizi kullanabilirsiniz.</p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <p style='color: #28a745; font-weight: bold;'>Kuaför ve Berberler Odası ailesine hoş geldiniz!</p>
                        </div>
                        
                        <hr style='border: none; border-top: 1px solid #dee2e6; margin: 30px 0;'>
                        <p style='font-size: 12px; color: #6c757d; text-align: center;'>
                            Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.
                        </p>
                    </div>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                // E-posta gönderilemezse sadece log'la, kullanıcıya hata gösterme
                // Log işlemi burada yapılabilir
                Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
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

            // Giriş başarılıysa hoş geldiniz e-postası gönder
            try
            {
                string loginTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                string subject = "Kuaför ve Berberler Odası - Giriş Bildirimi";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #2c3e50; text-align: center;'>Hoş Geldiniz!</h2>
                        <p>Sayın <strong>{user.NameSurName}</strong>,</p>
                        <p>Kuaför ve Berberler Odası sistemine başarıyla giriş yaptınız.</p>
                        
                        <div style='background-color: #e8f5e8; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #28a745;'>
                            <h4 style='color: #155724; margin-top: 0;'>Giriş Bilgileri:</h4>
                            <p><strong>Giriş Zamanı:</strong> {loginTime}</p>
                            <p><strong>Kullanıcı:</strong> {user.NameSurName} ({user.UserName})</p>
                        </div>
                        
                        <p>Sistemimizi güvenli bir şekilde kullanmaya devam edebilirsiniz.</p>
                        
                        <div style='background-color: #fff3cd; padding: 10px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #ffc107;'>
                            <p style='margin: 0; color: #856404;'><strong>Güvenlik Uyarısı:</strong> Eğer bu giriş sizin tarafınızdan yapılmadıysa, lütfen derhal şifrenizi değiştirin.</p>
                        </div>
                        
                        <hr style='border: none; border-top: 1px solid #dee2e6; margin: 30px 0;'>
                        <p style='font-size: 12px; color: #6c757d; text-align: center;'>
                            Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.
                        </p>
                    </div>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                // E-posta gönderilemezse sadece log'la, kullanıcıya hata gösterme
                Console.WriteLine($"Giriş bildirimi e-posta hatası: {ex.Message}");
            }

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