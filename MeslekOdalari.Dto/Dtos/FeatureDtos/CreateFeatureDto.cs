namespace MeslekOdalari.Dto.Dtos.FeatureDtos
{
    public class CreateFeatureDto
    {
        public string ImageUrl { get; set; }
        public bool MembershipStatus { get; set; }
        public int NumberofMembers { get; set; }
        public int NumberofProjects { get; set; }
        public string AreasofExpertise { get; set; }
    }
}
