using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.AuditBoardDtos
{
    public class UpdateAuditBoardDto
    {
        public ObjectId Id { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string SubTitle1 { get; set; }
        public string SubTitle2 { get; set; }
    }
}
