namespace MeslekOdalari.Entity.Entities
{
    public class News: BaseEntity
    {
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
