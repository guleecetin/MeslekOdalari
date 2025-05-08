using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.ProjectDtos
{
    public class UpdateProjectDto
    {
        public ObjectId Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
