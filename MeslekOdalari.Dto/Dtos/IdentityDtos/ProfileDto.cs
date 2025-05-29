using System.ComponentModel.DataAnnotations;
namespace MeslekOdalari.Dto.Dtos.IdentityDtos
{
    public class ProfileDto
    {
        [Display(Name = "TC Kimlik No")]
        public string TC { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı gereklidir")]
        [Display(Name = "Ad Soyad")]
        public string NameSurName { get; set; }

        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı gereklidir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegistrationDate { get; set; }
    }
}
