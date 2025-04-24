using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.VideoDtos
{
    public class UpdateVideoDto
    {
        public ObjectId Id { get; set; }
        public string VideoUrl { get; set; }
    }
}
