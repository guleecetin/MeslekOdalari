namespace MeslekOdalari.Dto.Dtos.RoomsDto
{
   public class CreateRoomsDto
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MemberCount { get; set; } //üye sayısı
        public string Email { get; set; }
        public string LicenseNumber { get; set; } //odanın lisans numarası
        public DateTime Established { get; set; } //odanın kurulduğu tarih
        public string PresidentName { get; set; } //başkanın adı
        public string PresidentImageUrl { get; set; } // Başkanın fotoğrafı
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
    }
}
