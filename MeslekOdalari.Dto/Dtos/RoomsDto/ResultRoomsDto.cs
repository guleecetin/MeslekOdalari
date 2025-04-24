using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.RoomsDto
{
    public class ResultRoomsDto
    {
        public ObjectId Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
    }
}
