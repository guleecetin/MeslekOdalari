using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.BoardDtos
{
    public class ResultBoardDto
    {
        public ObjectId Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string SubTitle1 { get; set; }
        public string SubTitle2 { get; set; }
    }
}
