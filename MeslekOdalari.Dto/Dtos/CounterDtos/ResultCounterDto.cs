using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.CounterDtos
{
   public class ResultCounterDto
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Count { get; set; }
    }
}
