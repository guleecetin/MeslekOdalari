using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.QuestDtos
{
    public class UpdateQuestDto
    {
        public ObjectId Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
