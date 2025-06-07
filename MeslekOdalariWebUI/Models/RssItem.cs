namespace MeslekOdalariWebUI.Models
{
    public class RssItem
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";
        public DateTime PublishDate { get; set; }
        public string Author { get; set; } = "";
        public List<string> Categories { get; set; } = new();
    }
}
