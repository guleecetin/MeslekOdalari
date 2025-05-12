using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.HistoryDtos
{
    public class UpdateHistoryDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
