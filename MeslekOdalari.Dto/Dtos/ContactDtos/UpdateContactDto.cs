using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.ContactDtos
{
    public class UpdateContactDto
    {
        public ObjectId Id { get; set; }
        public string MapUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
