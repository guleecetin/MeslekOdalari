using System.ComponentModel.DataAnnotations;

namespace MeslekOdalari.Dto.Dtos.IdentityDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "TC kimlik numarası gereklidir")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC kimlik numarası 11 hane olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Geçerli bir TC kimlik numarası giriniz")]
        public string TC { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }
    }
}
