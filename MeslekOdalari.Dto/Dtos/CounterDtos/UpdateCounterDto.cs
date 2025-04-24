using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.CounterDtos
{
    public class UpdateCounterDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Count { get; set; }
    }
}
