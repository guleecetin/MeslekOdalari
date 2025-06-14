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
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.RegularExpressions;

namespace MeslekOdalariWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            EmailService emailService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }


        #region Register Actions

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                         🔐 KULLANICI DOĞRULAMA                         🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var existingUserByTC = await _userManager.Users
                .FirstOrDefaultAsync(u => u.TC == registerDto.TC);

            if (existingUserByTC != null)
            {
                ModelState.AddModelError("TC", "Bu TC kimlik numarası ile kayıtlı kullanıcı bulunmaktadır.");
                return View(registerDto);
            }

            var existingUserByEmail = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUserByEmail != null)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi ile kayıtlı kullanıcı bulunmaktadır.");
                return View(registerDto);
            }

            var existingUserByUsername = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == registerDto.UserName);

            if (existingUserByUsername != null)
            {
                ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten kullanılmaktadır.");
                return View(registerDto);
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                       👤 YENİ KULLANICI OLUŞTUR                       🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var user = new AppUser
            {
                TC = registerDto.TC,
                NameSurName = registerDto.NameSurname,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                UserRole = UserRoles.Esnaf,
                IsApproved = false,
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(registerDto);
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                        📧 HOŞ GELDİNİZ E-POSTA                        🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            try
            {
                string subject = "🎉 Hoş Geldiniz - Kuaför ve Berberler Odası";
                string body = GenerateWelcomeEmailBody(user);

                await _emailService.SendEmailAsync(user.Email, subject, body);
                _logger.LogInformation($"✅ Hoş geldiniz e-postası gönderildi: {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ E-posta gönderme hatası: {user.Email}");
            }

            TempData["SuccessMessage"] = "Kayıt işleminiz başarıyla tamamlandı. Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        #endregion

        #region Login Actions

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                        🔍 KULLANICI DOĞRULAMA                         🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.TC == loginDto.TC);

            if (user == null)
            {
                ModelState.AddModelError("", "TC kimlik numarası veya şifre hatalı.");
                return View(loginDto);
            }

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "TC kimlik numarası veya şifre hatalı.");
                return View(loginDto);
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                          🎫 CLAIMS VE GİRİŞ                           🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
    new Claim("UserRole", ((int)user.UserRole).ToString()),
    new Claim("TC", user.TC),
    new Claim("UserId", user.Id.ToString()),
    new Claim("FullName", user.NameSurName ?? string.Empty)
};

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Identity.Application", claimsPrincipal);

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                       📩 GİRİŞ BİLDİRİM E-POSTA                       🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            try
            {
                string subject = "🔐 Güvenli Giriş Bildirimi - Kuaför ve Berberler Odası";
                string body = GenerateLoginNotificationEmailBody(user);

                await _emailService.SendEmailAsync(user.Email, subject, body);
                _logger.LogInformation($"✅ Giriş bildirimi e-postası gönderildi: {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Giriş bildirimi e-posta hatası: {user.Email}");
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                       🔄 ROL BAZLI YÖNLENDİRME                        🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            return user.UserRole == UserRoles.Admin
                ? RedirectToAction("Index", "Banner")
                : RedirectToAction("Index", "Default");
        }

        #endregion

        #region Profile Actions

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                         🔑 KİMLİK DOĞRULAMA                           🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ObjectId.TryParse(userIdString, out ObjectId userId))
                return RedirectToAction("Login");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                        📋 PROFİL DTO HAZIRLA                         🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
                return View(profileDto);

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                         🔑 KİMLİK DOĞRULAMA                           🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ObjectId.TryParse(userIdString, out ObjectId userId))
                return RedirectToAction("Login");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                       📧 E-POSTA ÇAKIŞMA KONTROLÜ                     🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            if (user.Email != profileDto.Email)
            {
                var existingUserByEmail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == profileDto.Email && u.Id != userId);

                if (existingUserByEmail != null)
                {
                    ViewBag.Message = "Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır.";
                    ViewBag.MessageType = "error";
                    return View(profileDto);
                }
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                       👤 KULLANICI ADI KONTROLÜ                       🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

            if (user.UserName != profileDto.UserName)
            {
                var existingUserByUsername = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.UserName == profileDto.UserName && u.Id != userId);

                if (existingUserByUsername != null)
                {
                    ViewBag.Message = "Bu kullanıcı adı başka bir kullanıcı tarafından kullanılmaktadır.";
                    ViewBag.MessageType = "error";
                    return View(profileDto);
                }
            }

            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵
            // 🔵                        💾 BİLGİLERİ GÜNCELLE                         🔵
            // 🔵━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━🔵

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
                    ModelState.AddModelError("", error.Description);
            }

            return View(profileDto);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordDto);
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.TC == forgotPasswordDto.TC && u.Email == forgotPasswordDto.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Girilen TC kimlik numarası ve e-posta adresi ile eşleşen bir kullanıcı bulunamadı.");
                return View(forgotPasswordDto);
            }

            try
            {
                var newPassword = GenerateSecurePassword();
                Console.WriteLine($"Kullanıcı: {user.Email} için yeni şifre oluşturuldu: {newPassword}");

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

                if (!resetResult.Succeeded)
                {
                    Console.WriteLine("Şifre sıfırlama hatası:");
                    foreach (var error in resetResult.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                    TempData["ErrorMessage"] = "Şifre sıfırlama işleminde bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return View(forgotPasswordDto);
                }

                string subject = "🔐 Yeni Şifreniz - Kuaför ve Berberler Odası";
                string body = GeneratePasswordEmailBody(user.NameSurName, newPassword);

                await _emailService.SendEmailAsync(user.Email, subject, body);

                TempData["SuccessMessage"] = "Yeni şifreniz e-posta adresinize gönderildi. Lütfen spam klasörünüzü de kontrol edin.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Şifre sıfırlama hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Şifre sıfırlama işleminde bir hata oluştu. Lütfen tekrar deneyiniz.";
                return View(forgotPasswordDto);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Default");
        }

        // Güvenli şifre oluşturma metodu
        private string GenerateSecurePassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%&*";

            var random = new Random();
            var password = new StringBuilder();

            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(numbers[random.Next(numbers.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            string allChars = upperCase + lowerCase + numbers + specialChars;
            for (int i = 4; i < 8; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            return new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());
        }

        // HOŞ GELDİNİZ E-POSTASI
        private string GenerateWelcomeEmailBody(AppUser user)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Hoş Geldiniz</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f5f5f5;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 8px 32px rgba(0,0,0,0.1);
        }}
        
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}
        
        .header h1 {{
            font-size: 32px;
            font-weight: 700;
            margin-bottom: 10px;
        }}
        
        .header p {{
            font-size: 18px;
            opacity: 0.9;
        }}
        
        .welcome-icon {{
            font-size: 80px;
            margin-bottom: 20px;
            animation: bounce 2s infinite;
        }}
        
        @keyframes bounce {{
            0%, 20%, 50%, 80%, 100% {{ transform: translateY(0); }}
            40% {{ transform: translateY(-10px); }}
            60% {{ transform: translateY(-5px); }}
        }}
        
        .content {{
            padding: 40px 30px;
        }}
        
        .greeting {{
            font-size: 20px;
            font-weight: 600;
            margin-bottom: 25px;
            color: #333;
        }}
        
        .message {{
            font-size: 16px;
            line-height: 1.8;
            margin-bottom: 30px;
            color: #555;
        }}
        
        .info-card {{
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border: 2px solid #667eea;
            border-radius: 15px;
            padding: 30px;
            margin: 30px 0;
        }}
        
        .info-card h3 {{
            color: #667eea;
            font-size: 20px;
            margin-bottom: 20px;
            text-align: center;
        }}
        
        .info-row {{
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 0;
            border-bottom: 1px solid #dee2e6;
        }}
        
        .info-row:last-child {{
            border-bottom: none;
        }}
        
        .info-label {{
            font-weight: 600;
            color: #495057;
        }}
        
        .info-value {{
            color: #667eea;
            font-weight: 500;
        }}
        
        .login-button {{
            display: inline-block;
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
            color: white !important;
            text-decoration: none;
            padding: 18px 40px;
            border-radius: 50px;
            font-size: 16px;
            font-weight: 600;
            margin: 30px auto;
            display: block;
            text-align: center;
            max-width: 250px;
            box-shadow: 0 4px 20px rgba(40, 167, 69, 0.3);
            transition: all 0.3s ease;
        }}
        
        .features {{
            background-color: #e8f4fd;
            border-radius: 12px;
            padding: 25px;
            margin: 30px 0;
        }}
        
        .features h4 {{
            color: #0066cc;
            font-size: 18px;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .feature-list {{
            list-style: none;
            padding: 0;
        }}
        
        .feature-list li {{
            padding: 8px 0;
            color: #0066cc;
            font-size: 14px;
        }}
        
        .feature-list li::before {{
            content: '✓';
            color: #28a745;
            font-weight: bold;
            margin-right: 10px;
        }}
        
        .footer {{
            background: linear-gradient(135deg, #343a40 0%, #495057 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        
        .footer p {{
            font-size: 14px;
            margin: 5px 0;
            opacity: 0.9;
        }}
        
        @media screen and (max-width: 600px) {{
            .email-container {{ margin: 0; border-radius: 0; }}
            .content, .header {{ padding: 30px 20px; }}
            .info-row {{ flex-direction: column; align-items: flex-start; }}
            .info-value {{ margin-top: 5px; }}
            .welcome-icon {{ font-size: 60px; }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <div class='welcome-icon'>🎉</div>
            <h1>Hoş Geldiniz!</h1>
            <p>Kuaför ve Berberler Odası</p>
        </div>
        
        <div class='content'>
            <div class='greeting'>
                Sayın {user.NameSurName},
            </div>
            
            <div class='message'>
                Kuaför ve Berberler Odası'na başarıyla kayıt oldunuz! Hesabınız oluşturulmuş ve artık tüm hizmetlerimizden yararlanabilirsiniz.
            </div>
            
            <div class='info-card'>
                <h3>📋 Hesap Bilgileriniz</h3>
                <div class='info-row'>
                    <span class='info-label'>👤 Ad Soyad:</span>
                    <span class='info-value'>{user.NameSurName}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>📧 E-posta:</span>
                    <span class='info-value'>{user.Email}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>🆔 Kullanıcı Adı:</span>
                    <span class='info-value'>{user.UserName}</span>
                </div>
                <div class='info-row'>
                    <span class='info-label'>📅 Kayıt Tarihi:</span>
                    <span class='info-value'>{user.RegistrationDate:dd.MM.yyyy HH:mm}</span>
                </div>
            </div>
            
            <div class='features'>
                <h4>🌟 Neler Yapabilirsiniz?</h4>
                <ul class='feature-list'>
                    <li>Oda faaliyetlerini takip edebilirsiniz</li>
                    <li>Duyuru ve haberlerden haberdar olabilirsiniz</li>
                    <li>Üye hizmetlerinden yararlanabilirsiniz</li>
                    <li>İletişim kanallarımızı kullanabilirsiniz</li>
                    <li>Profil bilgilerinizi güncelleyebilirsiniz</li>
                </ul>
            </div>
            
            <a href='#' class='login-button'>
                🚀 Hemen Giriş Yap
            </a>
            
            <div style='text-align: center; margin-top: 30px; padding: 20px; background-color: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;'>
                <p style='margin: 0; color: #856404; font-weight: 600;'>
                    💡 Giriş yapmak için TC Kimlik numaranız ve belirlediğiniz şifreyi kullanabilirsiniz.
                </p>
            </div>
        </div>
        
        <div class='footer'>
            <p><strong>Kuaför ve Berberler Odası ailesine hoş geldiniz!</strong></p>
            <p>Bu e-posta otomatik olarak gönderilmiştir.</p>
            <p>© 2024 Kuaför ve Berberler Odası - Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        // GİRİŞ BİLDİRİMİ E-POSTASI
        private string GenerateLoginNotificationEmailBody(AppUser user)
        {
            string loginTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Giriş Bildirimi</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f5f5f5;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 8px 32px rgba(0,0,0,0.1);
        }}
        
        .header {{
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
            padding: 30px 20px;
            text-align: center;
            color: white;
        }}
        
        .header h1 {{
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 10px;
        }}
        
        .login-icon {{
            font-size: 60px;
            margin-bottom: 15px;
            animation: pulse 2s infinite;
        }}
        
        @keyframes pulse {{
            0% {{ transform: scale(1); }}
            50% {{ transform: scale(1.1); }}
            100% {{ transform: scale(1); }}
        }}
        
        .content {{
            padding: 40px 30px;
        }}
        
        .greeting {{
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 20px;
            color: #333;
        }}
        
        .message {{
            font-size: 16px;
            line-height: 1.8;
            margin-bottom: 30px;
            color: #555;
        }}
        
        .login-info {{
            background: linear-gradient(135deg, #e8f5e8 0%, #d4edda 100%);
            border: 2px solid #28a745;
            border-radius: 15px;
            padding: 25px;
            margin: 25px 0;
        }}
        
        .login-info h3 {{
            color: #155724;
            font-size: 18px;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .info-item {{
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 10px 0;
            border-bottom: 1px solid #c3e6cb;
        }}
        
        .info-item:last-child {{
            border-bottom: none;
        }}
        
        .info-label {{
            font-weight: 600;
            color: #155724;
        }}
        
        .info-value {{
            color: #28a745;
            font-weight: 500;
        }}
        
        .security-warning {{
            background-color: #fff3cd;
            border: 2px solid #ffc107;
            border-radius: 12px;
            padding: 20px;
            margin: 25px 0;
        }}
        
        .security-warning h4 {{
            color: #856404;
            font-size: 16px;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
        }}
        
        .security-warning p {{
            color: #856404;
            font-size: 14px;
            margin: 8px 0;
        }}
        
        .action-buttons {{
            text-align: center;
            margin: 30px 0;
        }}
        
        .btn {{
            display: inline-block;
            padding: 12px 25px;
            border-radius: 25px;
            text-decoration: none;
            font-weight: 600;
            margin: 5px 10px;
            transition: all 0.3s ease;
        }}
        
        .btn-primary {{
            background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
            color: white;
            box-shadow: 0 4px 15px rgba(0, 123, 255, 0.3);
        }}
        
        .btn-secondary {{
            background: linear-gradient(135deg, #6c757d 0%, #545b62 100%);
            color: white;
            box-shadow: 0 4px 15px rgba(108, 117, 125, 0.3);
        }}
        
        .footer {{
            background: linear-gradient(135deg, #343a40 0%, #495057 100%);
            color: white;
            padding: 25px;
            text-align: center;
        }}
        
        .footer p {{
            font-size: 13px;
            margin: 3px 0;
            opacity: 0.9;
        }}
        
        @media screen and (max-width: 600px) {{
            .email-container {{ margin: 0; border-radius: 0; }}
            .content, .header {{ padding: 25px 15px; }}
            .info-item {{ flex-direction: column; align-items: flex-start; }}
            .info-value {{ margin-top: 5px; }}
            .btn {{ display: block; margin: 10px 0; }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <div class='login-icon'>🔐</div>
            <h2>Güvenli Giriş Bildirimi</h2>
            <p>Kuaför ve Berberler Odası</p>
        </div>
        
        <div class='content'>
            <div class='greeting'>
                Sayın {user.NameSurName},
            </div>
            
            <div class='message'>
                Hesabınıza başarıyla giriş yapıldı. Sistemimizi güvenli bir şekilde kullanmaya devam edebilirsiniz.
            </div>
            
            <div class='login-info'>
                <h3>🔍 Giriş Detayları</h3>
                <div class='info-item'>
                    <span class='info-label'>⏰ Giriş Zamanı:</span>
                    <span class='info-value'>{loginTime}</span>
                </div>
                <div class='info-item'>
                    <span class='info-label'>👤 Kullanıcı:</span>
                    <span class='info-value'>{user.NameSurName}</span>
                </div>
                <div class='info-item'>
                    <span class='info-label'>🆔 Kullanıcı Adı:</span>
                    <span class='info-value'>{user.UserName}</span>
                </div>
                <div class='info-item'>
                    <span class='info-label'>📧 E-posta:</span>
                    <span class='info-value'>{user.Email}</span>
                </div>
            </div>
            
            <div class='security-warning'>
                <h4>
                    ⚠️ Güvenlik Uyarısı
                </h4>
                <p><strong>Bu giriş sizin tarafınızdan yapılmadıysa:</strong></p>
                <p>• Derhal şifrenizi değiştirin</p>
                <p>• Hesap güvenliğinizi gözden geçirin</p>
                <p>• Şüpheli aktivite tespit ederseniz bizimle iletişime geçin</p>
            </div>
          
            
            <div style='text-align: center; margin-top: 25px; padding: 15px; background-color: #e7f3ff; border-radius: 8px; border-left: 4px solid #007bff;'>
                <p style='margin: 0; color: #004085; font-size: 14px;'>
                    🛡️ Hesap güvenliğiniz bizim için önemlidir. Şüpheli bir durum fark ederseniz lütfen bizimle iletişime geçin.
                </p>
            </div>
        </div>
        
        <div class='footer'>
            <p><strong>Güvenli bir deneyim için teşekkürler!</strong></p>
            <p>Bu e-posta otomatik olarak gönderilmiştir.</p>
            <p>© 2024 Kuaför ve Berberler Odası - Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        // ŞİFRE SIFIRLAMA E-POSTASI
        private string GeneratePasswordEmailBody(string userName, string newPassword)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Yeni Şifreniz</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f5f5f5;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 8px 32px rgba(0,0,0,0.1);
        }}
        
        .header {{
            background: linear-gradient(135deg, #dc3545 0%, #c82333 100%);
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}
        
        .header h1 {{
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 10px;
        }}
        
        .header p {{
            font-size: 16px;
            opacity: 0.9;
        }}
        
        .password-icon {{
            font-size: 60px;
            margin-bottom: 20px;
            animation: shake 0.5s ease-in-out infinite alternate;
        }}
        
        @keyframes shake {{
            0% {{ transform: translateX(0px); }}
            100% {{ transform: translateX(2px); }}
        }}
        
        .content {{
            padding: 40px 30px;
        }}
        
        .greeting {{
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 25px;
            color: #333;
        }}
        
        .message {{
            font-size: 16px;
            line-height: 1.8;
            margin-bottom: 30px;
            color: #555;
        }}
        
        .password-card {{
            background: linear-gradient(135deg, #fff5f5 0%, #ffe6e6 100%);
            border: 3px solid #dc3545;
            border-radius: 15px;
            padding: 30px;
            margin: 30px 0;
            text-align: center;
        }}
        
        .password-card h3 {{
            color: #dc3545;
            font-size: 20px;
            margin-bottom: 20px;
        }}
        
        .password-display {{
            background-color: #fff;
            border: 2px solid #dc3545;
            border-radius: 10px;
            padding: 20px;
            margin: 20px 0;
            font-family: 'Courier New', monospace;
            font-size: 24px;
            font-weight: bold;
            color: #dc3545;
            letter-spacing: 2px;
            word-break: break-all;
            box-shadow: 0 4px 15px rgba(220, 53, 69, 0.2);
        }}
        
        .copy-instruction {{
            background-color: #d1ecf1;
            color: #0c5460;
            padding: 15px;
            border-radius: 8px;
            margin: 20px 0;
            border-left: 4px solid #17a2b8;
        }}
        
        .copy-instruction strong {{
            color: #0c5460;
        }}
        
        .security-steps {{
            background-color: #fff3cd;
            border: 2px solid #ffc107;
            border-radius: 12px;
            padding: 25px;
            margin: 30px 0;
        }}
        
        .security-steps h4 {{
            color: #856404;
            font-size: 18px;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .steps-list {{
            list-style: none;
            padding: 0;
            counter-reset: step-counter;
        }}
        
        .steps-list li {{
            counter-increment: step-counter;
            padding: 12px 0;
            color: #856404;
            font-size: 14px;
            position: relative;
            padding-left: 40px;
        }}
        
        .steps-list li::before {{
            content: counter(step-counter);
            position: absolute;
            left: 0;
            top: 50%;
            transform: translateY(-50%);
            background-color: #ffc107;
            color: #856404;
            font-weight: bold;
            width: 25px;
            height: 25px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
        }}
        
        .login-button {{
            display: inline-block;
            background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
            color: white !important;
            text-decoration: none;
            padding: 18px 40px;
            border-radius: 50px;
            font-size: 16px;
            font-weight: 600;
            margin: 30px auto;
            display: block;
            text-align: center;
            max-width: 250px;
            box-shadow: 0 4px 20px rgba(0, 123, 255, 0.3);
            transition: all 0.3s ease;
        }}
        
        .important-note {{
            background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
            color: white;
            padding: 25px;
            border-radius: 12px;
            margin: 30px 0;
            text-align: center;
        }}
        
        .important-note h4 {{
            font-size: 18px;
            margin-bottom: 15px;
        }}
        
        .important-note p {{
            font-size: 14px;
            margin: 8px 0;
            opacity: 0.95;
        }}
        
        .footer {{
            background: linear-gradient(135deg, #343a40 0%, #495057 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        
        .footer p {{
            font-size: 14px;
            margin: 5px 0;
            opacity: 0.9;
        }}
        
        @media screen and (max-width: 600px) {{
            .email-container {{ margin: 0; border-radius: 0; }}
            .content, .header {{ padding: 30px 20px; }}
            .password-display {{ font-size: 18px; letter-spacing: 1px; }}
            .steps-list li {{ padding-left: 35px; }}
            .password-icon {{ font-size: 50px; }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <div class='password-icon'>🔐</div>
            <h1>Yeni Şifreniz</h1>
            <p>Kuaför ve Berberler Odası</p>
        </div>
        
        <div class='content'>
            <div class='greeting'>
                Sayın {userName},
            </div>
            
            <div class='message'>
                Şifre sıfırlama talebiniz başarıyla işleme alınmıştır. Aşağıda yeni şifrenizi bulabilirsiniz.
            </div>
            
            <div class='password-card'>
                <h3>🔑 Yeni Şifreniz</h3>
                <div class='password-display'>
                    {newPassword}
                </div>
                <div class='copy-instruction'>
                    <strong>💡 İpucu:</strong> Şifreyi kopyalayıp yapıştırarak kullanabilirsiniz. Büyük-küçük harf duyarlılığına dikkat edin.
                </div>
            </div>
            
            <div class='security-steps'>
                <h4>🛡️ Güvenlik İçin Önerilen Adımlar</h4>
                <ol class='steps-list'>
                    <li>Bu şifre ile sisteme giriş yapın</li>
                    <li>Giriş yaptıktan sonra hemen şifrenizi değiştirin</li>
                    <li>Güçlü ve size özel bir şifre belirleyin</li>
                    <li>Şifrenizi kimseyle paylaşmayın</li>
                    <li>Bu e-postayı güvenli bir şekilde saklayın veya silin</li>
                </ol>
            </div>
            
            <a href='#' class='login-button'>
                🚀 Hemen Giriş Yap
            </a>
            
            <div class='important-note'>
                <h4>⚠️ ÖNEMLİ GÜVENLİK UYARISI</h4>
                <p><strong>Bu şifre geçicidir ve değiştirilmelidir!</strong></p>
                <p>Güvenliğiniz için ilk girişinizde mutlaka yeni bir şifre belirleyin.</p>
                <p>Şifre sıfırlama talebini siz yapmadıysanız derhal bizimle iletişime geçin.</p>
            </div>
            
            <div style='text-align: center; margin-top: 30px; padding: 20px; background-color: #e7f3ff; border-radius: 8px; border-left: 4px solid #007bff;'>
                <p style='margin: 0; color: #004085; font-weight: 600; font-size: 14px;'>
                    🔒 Hesap güvenliğiniz bizim önceliğimizdir. Şüpheli bir durum fark ederseniz lütfen destek ekibimizle iletişime geçin.
                </p>
            </div>
        </div>
        
        <div class='footer'>
            <p><strong>Güvenli bir deneyim için teşekkürler!</strong></p>
            <p>Bu e-posta şifre sıfırlama talebi üzerine otomatik olarak gönderilmiştır.</p>
            <p>© 2024 Kuaför ve Berberler Odası - Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}