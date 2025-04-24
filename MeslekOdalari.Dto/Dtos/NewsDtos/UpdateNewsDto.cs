using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.NewsDtos
{
    public class UpdateNewsDto
    {
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
