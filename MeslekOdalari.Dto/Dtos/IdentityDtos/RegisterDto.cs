using Microsoft.AspNetCore.Http;

namespace MeslekOdalari.Dto.Dtos.IdentityDtos
{
    public class RegisterDto
    {
        public string TC { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public IFormFile CopyOfIDCard { get; set; }            // Nüfus Cüzdanı Fotokopisi
        public IFormFile ResidenceCertificate { get; set; }    // İkametgah Belgesi
        public IFormFile TaxPlate { get; set; }                 // Vergi Levhası
        public IFormFile BusinessOpeningLicense { get; set; }  // İşyeri Açma ve Çalışma Ruhsatı
        public IFormFile RegistrationForm { get; set; }        // Oda Kayıt Formu
        public IFormFile DiplomaOrCertification { get; set; }  // Diploma veya Mesleki Yeterlilik Belgesi
        public IFormFile TradesmanRegistryDeclaration { get; set; } // Esnaf Sicil Beyannamesi
        public IFormFile SignatureDeclaration { get; set; }    // İmza Beyannamesi
    }
}
