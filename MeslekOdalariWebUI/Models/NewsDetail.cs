namespace MeslekOdalariWebUI.Models
{
    public class NewsDetail
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImageUrl { get; set; } //resim
        public string NewsUrl { get; set; } //haber
    }
}
