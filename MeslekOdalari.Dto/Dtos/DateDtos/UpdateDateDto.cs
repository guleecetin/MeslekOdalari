using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.DateDtos
{
   public class UpdateDateDto
    {
        public ObjectId Id { get; set; }
        public string RandevuNo { get; set; }
        public string HizmetTuru { get; set; }
        public string HizmetDetay { get; set; }
        public DateTime RandevuTarih { get; set; }
        public string RandevuSaat { get; set; }
        public string AdSoyad { get; set; }
        public string TcNo { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string IsletmeAdi { get; set; }
        public string SicilNo { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public string Durum { get; set; } = "aktif";
    }
}
