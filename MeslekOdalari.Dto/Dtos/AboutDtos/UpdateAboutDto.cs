using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.AboutDtos
{
    public class UpdateAboutDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
