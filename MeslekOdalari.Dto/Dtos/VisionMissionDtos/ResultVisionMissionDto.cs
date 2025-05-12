using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.VisionMissionDtos
{
    public class ResultVisionMissionDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
