namespace MeslekOdalari.Entity.Entities
{
    public class Feature:BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool MembershipStatus { get; set; }
        public int NumberofMembers { get; set; }
        public int NumberofProjects { get; set; }
        public string AreasofExpertise { get; set; }

    }
}
