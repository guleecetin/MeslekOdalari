using System.ComponentModel.DataAnnotations;

namespace MeslekOdalari.Dto.Dtos.IdentityDtos
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TC { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
