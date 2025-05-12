using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.HistoryDtos
{
    public class ResultHistoryDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
