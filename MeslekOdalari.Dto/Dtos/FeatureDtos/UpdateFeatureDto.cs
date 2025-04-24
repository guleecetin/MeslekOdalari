using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.FeatureDtos
{
   public class UpdateFeatureDto
    {
        public ObjectId Id { get; set; }
        public string ImageUrl { get; set; }
        public string MembershipStatus { get; set; }
        public int NumberofMembers { get; set; }
        public int NumberofProjects { get; set; }
        public string AreasofExpertise { get; set; }
    }
}
