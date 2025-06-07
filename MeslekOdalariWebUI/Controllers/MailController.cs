using MeslekOdalariWebUI.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeslekOdalariWebUI.Controllers
{
    public class MailController : Controller
    {
        private readonly EmailService _emailService;

        public MailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // Randevu onayı için (mevcut)
        [HttpPost]
        public async Task<IActionResult> SendMail(string musteriMail)
        {
            string subject = "Kuaför ve Berberler Odası Randevu Onayı";
            string body = "<h3>Rezervasyonunuz başarıyla alınmıştır.</h3>";
            await _emailService.SendEmailAsync(musteriMail, subject, body);
            return Content("Mail başarıyla gönderildi.");
        }

        // Randevu onayı - Geliştirilmiş versiyon
        [HttpPost]
        public async Task<IActionResult> SendAppointmentConfirmation(string musteriMail, string musteriAd, string randevuTarihi, string hizmet, string berberAdi)
        {
            try
            {
                string subject = "Kuaför ve Berberler Odası - Randevu Onayı";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #2c3e50; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h2 style='margin: 0;'>Randevu Onayı</h2>
                        </div>
                        
                        <div style='background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px;'>
                            <p>Sayın <strong>{musteriAd}</strong>,</p>
                            <p>Randevunuz başarıyla alınmıştır. Aşağıdaki bilgileri kontrol ediniz:</p>
                            
                            <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #28a745;'>
                                <h4 style='color: #28a745; margin-top: 0;'>Randevu Detayları</h4>
                                <p><strong>📅 Tarih & Saat:</strong> {randevuTarihi}</p>
                                <p><strong>✂️ Hizmet:</strong> {hizmet}</p>
                                <p><strong>👨‍💼 Berber:</strong> {berberAdi}</p>
                            </div>
                            
                            <div style='background-color: #fff3cd; padding: 15px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #ffc107;'>
                                <p style='margin: 0; color: #856404;'><strong>⚠️ Önemli:</strong> Randevunuza 15 dakika erkenden gelmenizi rica ederiz.</p>
                            </div>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <p style='color: #28a745; font-weight: bold;'>Randevunuz için teşekkür ederiz!</p>
                            </div>
                        </div>
                        
                        <div style='text-align: center; padding: 20px; font-size: 12px; color: #6c757d;'>
                            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            <p>Kuaför ve Berberler Odası</p>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(musteriMail, subject, body);
                return Json(new { success = true, message = "Randevu onay maili başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mail gönderilirken hata oluştu: " + ex.Message });
            }
        }

        // Randevu iptali bildirimi
        [HttpPost]
        public async Task<IActionResult> SendAppointmentCancellation(string musteriMail, string musteriAd, string randevuTarihi, string iptalNedeni = null)
        {
            try
            {
                string subject = "Kuaför ve Berberler Odası - Randevu İptali";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #dc3545; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h2 style='margin: 0;'>Randevu İptali</h2>
                        </div>
                        
                        <div style='background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px;'>
                            <p>Sayın <strong>{musteriAd}</strong>,</p>
                            <p>Aşağıdaki randevunuz iptal edilmiştir:</p>
                            
                            <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #dc3545;'>
                                <h4 style='color: #dc3545; margin-top: 0;'>İptal Edilen Randevu</h4>
                                <p><strong>📅 Tarih & Saat:</strong> {randevuTarihi}</p>
                                {(string.IsNullOrEmpty(iptalNedeni) ? "" : $"<p><strong>📝 İptal Nedeni:</strong> {iptalNedeni}</p>")}
                            </div>
                            
                            <p>Yeni bir randevu almak için sistemimizi kullanabilirsiniz.</p>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <p style='color: #dc3545;'>Anlayışınız için teşekkür ederiz.</p>
                            </div>
                        </div>
                        
                        <div style='text-align: center; padding: 20px; font-size: 12px; color: #6c757d;'>
                            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            <p>Kuaför ve Berberler Odası</p>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(musteriMail, subject, body);
                return Json(new { success = true, message = "İptal bildirimi başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mail gönderilirken hata oluştu: " + ex.Message });
            }
        }

        // Randevu hatırlatma maili
        [HttpPost]
        public async Task<IActionResult> SendAppointmentReminder(string musteriMail, string musteriAd, string randevuTarihi, string hizmet, string berberAdi)
        {
            try
            {
                string subject = "Kuaför ve Berberler Odası - Randevu Hatırlatması";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #17a2b8; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h2 style='margin: 0;'>🔔 Randevu Hatırlatması</h2>
                        </div>
                        
                        <div style='background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px;'>
                            <p>Sayın <strong>{musteriAd}</strong>,</p>
                            <p>Yarınki randevunuzu hatırlatmak istiyoruz:</p>
                            
                            <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #17a2b8;'>
                                <h4 style='color: #17a2b8; margin-top: 0;'>Yarınki Randevunuz</h4>
                                <p><strong>📅 Tarih & Saat:</strong> {randevuTarihi}</p>
                                <p><strong>✂️ Hizmet:</strong> {hizmet}</p>
                                <p><strong>👨‍💼 Berber:</strong> {berberAdi}</p>
                            </div>
                            
                            <div style='background-color: #d4edda; padding: 15px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #28a745;'>
                                <p style='margin: 0; color: #155724;'><strong>💡 Hatırlatma:</strong> Randevunuza 15 dakika erkenden gelmeyi unutmayın!</p>
                            </div>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <p>Sizi görmek için sabırsızlanıyoruz! 😊</p>
                            </div>
                        </div>
                        
                        <div style='text-align: center; padding: 20px; font-size: 12px; color: #6c757d;'>
                            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            <p>Kuaför ve Berberler Odası</p>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(musteriMail, subject, body);
                return Json(new { success = true, message = "Hatırlatma maili başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mail gönderilirken hata oluştu: " + ex.Message });
            }
        }

        // Genel bilgilendirme maili
        [HttpPost]
        public async Task<IActionResult> SendGeneralNotification(string musteriMail, string musteriAd, string konu, string mesaj)
        {
            try
            {
                string subject = $"Kuaför ve Berberler Odası - {konu}";
                string body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #6c757d; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h2 style='margin: 0;'>{konu}</h2>
                        </div>
                        
                        <div style='background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px;'>
                            <p>Sayın <strong>{musteriAd}</strong>,</p>
                            
                            <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #6c757d;'>
                                <p>{mesaj}</p>
                            </div>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <p>İyi günler dileriz! 🌟</p>
                            </div>
                        </div>
                        
                        <div style='text-align: center; padding: 20px; font-size: 12px; color: #6c757d;'>
                            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                            <p>Kuaför ve Berberler Odası</p>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(musteriMail, subject, body);
                return Json(new { success = true, message = "Bilgilendirme maili başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mail gönderilirken hata oluştu: " + ex.Message });
            }
        }
    }
}