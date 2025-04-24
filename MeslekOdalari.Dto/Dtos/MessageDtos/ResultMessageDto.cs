using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.MessageDtos
{
    public class ResultMessageDto
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
