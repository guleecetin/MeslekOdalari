// Services/RssService.cs
using System.ServiceModel.Syndication;
using System.Xml;
using MeslekOdalariWebUI.Models;

namespace MeslekOdalariWebUI.Services
{
    public interface IRssService
    {
        Task<List<RssItem>> GetRssFeedAsync(string rssUrl);
    }

    public class RssService : IRssService
    {
        private readonly HttpClient _httpClient;

        public RssService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RssItem>> GetRssFeedAsync(string rssUrl)
        {
            var rssItems = new List<RssItem>();

            try
            {
                // RSS URL'inden veri çek
                var response = await _httpClient.GetAsync(rssUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // XML'i oku
                using var stringReader = new StringReader(content);
                using var xmlReader = XmlReader.Create(stringReader);

                var feed = SyndicationFeed.Load(xmlReader);

                // Her bir RSS öğesini işle
                foreach (var item in feed.Items)
                {
                    rssItems.Add(new RssItem
                    {
                        Title = item.Title?.Text ?? "",
                        Description = item.Summary?.Text ?? "",
                        Link = item.Links?.FirstOrDefault()?.Uri?.ToString() ?? "",
                        PublishDate = item.PublishDate.DateTime,
                        Author = item.Authors?.FirstOrDefault()?.Name ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda konsola yaz
                Console.WriteLine($"RSS okuma hatası: {ex.Message}");
            }

            return rssItems.OrderByDescending(x => x.PublishDate).ToList();
        }
    }
}