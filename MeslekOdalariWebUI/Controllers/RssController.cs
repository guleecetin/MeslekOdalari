// Controllers/RssController.cs
using Microsoft.AspNetCore.Mvc;
using MeslekOdalariWebUI.Services;
using MeslekOdalariWebUI.Helpers;

namespace MeslekOdalariWebUI.Controllers
{
    public class RssController : Controller
    {
        private readonly IRssService _rssService;
        private readonly ILogger<RssController> _logger;

        public RssController(IRssService rssService, ILogger<RssController> logger)
        {
            _rssService = rssService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Farklı RSS kaynaklarını dene
            var rssUrls = new[]
            {
                "https://www.haberturk.com/rss",
                "https://www.cnnturk.com/feed/rss/news",
                "https://www.sabah.com.tr/rss/anasayfa.xml",
                "https://www.milliyet.com.tr/rss/rssnew/gundemrss.xml"
            };

            foreach (var rssUrl in rssUrls)
            {
                try
                {
                    _logger.LogInformation($"RSS URL'si deneniyor: {rssUrl}");

                    var allRssItems = await _rssService.GetRssFeedAsync(rssUrl);

                    if (allRssItems != null && allRssItems.Any())
                    {
                        _logger.LogInformation($"Başarılı! {allRssItems.Count} haber alındı: {rssUrl}");

                        // İlk olarak Elazığ haberlerini filtrele
                        var elazigNews = FilterElazigNews(allRssItems);

                        // Eğer Elazığ haberi az ise, genel haberleri göster
                        var itemsToShow = elazigNews.Count >= 3 ? elazigNews : allRssItems;

                        var sortedItems = itemsToShow
                            .OrderByDescending(x => x.PublishDate)
                            .Take(20)
                            .ToList();

                        ViewBag.FeedTitle = "Güncel Haberler";
                        ViewBag.TotalNews = sortedItems.Count;
                        ViewBag.ElazigNewsCount = elazigNews.Count;
                        ViewBag.TotalFetched = allRssItems.Count;
                        ViewBag.ActiveRssUrl = rssUrl;

                        return View(sortedItems);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"RSS URL'si başarısız: {rssUrl}");
                    continue;
                }
            }

            // Hiçbir RSS çalışmadıysa test verileri göster
            ViewBag.ErrorMessage = "RSS beslemeleri şu anda erişilemez durumda. Test verileri gösteriliyor.";
            ViewBag.FeedTitle = "Test Haberleri";

            return View(GetTestData());
        }

        private List<Models.RssItem> GetTestData()
        {
            return new List<Models.RssItem>
            {
                new Models.RssItem
                {
                    Title = "Elazığ'da Yeni Projelerin Temeli Atıldı",
                    Description = "Elazığ Belediyesi tarafından şehrin çeşitli bölgelerinde yeni projeler başlatıldı.",
                    Link = "#",
                    PublishDate = DateTime.Now.AddMinutes(-30),
                    Author = "Test Editör"
                },
                new Models.RssItem
                {
                    Title = "Merkez İlçede Kültür Etkinlikleri",
                    Description = "Elazığ merkez ilçede düzenlenen kültür etkinlikleri vatandaşlar tarafından büyük ilgi gördü.",
                    Link = "#",
                    PublishDate = DateTime.Now.AddHours(-2),
                    Author = "Kültür Muhabiri"
                },
                new Models.RssItem
                {
                    Title = "Sivrice'de Tarım Fuarı Açıldı",
                    Description = "Sivrice ilçesinde düzenlenen tarım fuarı çiftçiler ve ziyaretçiler tarafından yoğun ilgi gördü.",
                    Link = "#",
                    PublishDate = DateTime.Now.AddHours(-4),
                    Author = "Tarım Muhabiri"
                }
            };
        }

        // Sadece Elazığ ile ilgili haberleri filtrele
        private List<Models.RssItem> FilterElazigNews(List<Models.RssItem> items)
        {
            var elazigKeywords = new List<string>
            {
                "elazığ", "elâzığ", "elazig", "merkez", "sivrice", "karakoçan",
                "palu", "maden", "arıcak", "keban", "ağın", "baskil",
                "alacakaya", "kovancılar"
            };

            return items.Where(item =>
                ContainsElazigKeyword(item.Title, elazigKeywords) ||
                ContainsElazigKeyword(item.Description, elazigKeywords)
            ).ToList();
        }

        private bool ContainsElazigKeyword(string text, List<string> keywords)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            text = text.ToLowerInvariant()
                      .Replace('ı', 'i')
                      .Replace('ğ', 'g')
                      .Replace('ü', 'u')
                      .Replace('ş', 's')
                      .Replace('ç', 'c')
                      .Replace('ö', 'o');

            return keywords.Any(keyword =>
                text.Contains(keyword.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));
        }

        // Tüm haberleri getir (filtre olmadan)
        public async Task<IActionResult> AllNews()
        {
            var rssUrls = new[]
            {
                "https://www.haberturk.com/rss",
                "https://www.cnnturk.com/feed/rss/news",
                "https://www.sabah.com.tr/rss/anasayfa.xml"
            };

            foreach (var rssUrl in rssUrls)
            {
                try
                {
                    var rssItems = await _rssService.GetRssFeedAsync(rssUrl);

                    if (rssItems != null && rssItems.Any())
                    {
                        var sortedItems = rssItems
                            .OrderByDescending(x => x.PublishDate)
                            .Take(30)
                            .ToList();

                        ViewBag.FeedTitle = "Tüm Haberler";
                        ViewBag.ShowAllNews = true;
                        ViewBag.ActiveRssUrl = rssUrl;

                        return View("Index", sortedItems);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"AllNews RSS hatası: {rssUrl}");
                    continue;
                }
            }

            ViewBag.ErrorMessage = "Haberler yüklenirken bir hata oluştu.";
            return View("Index", new List<Models.RssItem>());
        }

        // Debug için RSS durumunu kontrol et
        public async Task<IActionResult> Debug()
        {
            var testUrls = new[]
            {
                "https://www.haberturk.com/rss",
                "https://www.cnnturk.com/feed/rss/news",
                "https://www.sabah.com.tr/rss/anasayfa.xml",
                "https://kanal23.com/rss"
            };

            var results = new List<string>();

            foreach (var url in testUrls)
            {
                try
                {
                    var items = await _rssService.GetRssFeedAsync(url);
                    results.Add($"✅ {url}: {items?.Count ?? 0} haber");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ {url}: {ex.Message}");
                }
            }

            ViewBag.DebugResults = results;
            return View();
        }

        public async Task<IActionResult> NewsDetail(string url, string title, string description, string date, int id)
        {
            if (string.IsNullOrEmpty(url))
            {
                return RedirectToAction("Index");
            }

            try
            {
                // URL'den içerik çekmeye çalış
                var webContent = await ExtractNewsContent(url);

                // Description'ı decode et ve temizle
                var cleanDescription = Uri.UnescapeDataString(description ?? "");

                // Eğer web içeriği yeterli değilse, description'ı genişlet
                string finalContent;
                if (IsContentSufficient(webContent))
                {
                    finalContent = webContent;
                }
                else
                {
                    // Description'ı ana içerik olarak kullan ve geliştir
                    finalContent = ExpandDescriptionContent(cleanDescription, webContent);
                }

                // Default değerleri tanımla - bu değerleri ihtiyacınıza göre ayarlayın
                string imageUrl = "/images/default-news.jpg"; // Default resim
                string articleUrl = url; // Orijinal haber URL'si

                // Eğer bu değerleri parametre olarak almak istiyorsanız:
                // imageUrl = !string.IsNullOrEmpty(imageParam) ? imageParam : "/images/default-news.jpg";
                // articleUrl = !string.IsNullOrEmpty(originalUrlParam) ? originalUrlParam : url;

                var newsDetail = new Models.NewsDetail
                {
                    Title = Uri.UnescapeDataString(title ?? ""),
                    Description = cleanDescription,
                    Content = finalContent,
                    ImageUrl = imageUrl,
                    NewsUrl = articleUrl,
                    PublishDate = DateTime.TryParse(date, out var parsedDate) ? parsedDate : DateTime.Now
                };

                return View(newsDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Haber detayı alınırken hata: {url}");

                // Hata durumunda da değişkenleri tanımla
                string imageUrl = "/images/default-news.jpg";
                string articleUrl = url;

                // Hata durumunda description'ı içerik olarak kullan
                var basicDetail = new Models.NewsDetail
                {
                    Title = Uri.UnescapeDataString(title ?? "Haber Başlığı"),
                    Description = Uri.UnescapeDataString(description ?? "Haber açıklaması mevcut değil."),
                    Content = Uri.UnescapeDataString(description ?? "İçerik şu anda yüklenemiyor. Lütfen orijinal haberi okumak için aşağıdaki linke tıklayın."),
                    ImageUrl = imageUrl,
                    NewsUrl = articleUrl,
                    PublishDate = DateTime.TryParse(date, out var parsedDate) ? parsedDate : DateTime.Now
                };

                return View(basicDetail);
            }
        }

        private bool IsContentSufficient(string content)
        {
            return !string.IsNullOrEmpty(content) &&
                   content.Length > 200 &&
                   !content.Contains("formatında değil") &&
                   !content.Contains("yüklenemedi") &&
                   !content.Contains("hata oluştu") &&
                   !content.Contains("erişilemiyor");
        }

        private string ExpandDescriptionContent(string description, string webContent)
        {
            var expandedContent = new System.Text.StringBuilder();

            // Description'ı ana paragraf olarak ekle
            if (!string.IsNullOrEmpty(description))
            {
                expandedContent.AppendLine(description);
                expandedContent.AppendLine();
            }

            // Web içeriği kullanılabilirse ekle
            if (!string.IsNullOrEmpty(webContent) &&
                !webContent.Contains("formatında değil") &&
                !webContent.Contains("yüklenemedi"))
            {
                expandedContent.AppendLine("--- Ek Bilgiler ---");
                expandedContent.AppendLine(webContent);
            }
            else
            {
                // Web içeriği yoksa genel bilgilendirme ekle
                expandedContent.AppendLine("Bu haberin detayları için orijinal kaynağa başvurabilirsiniz.");
                expandedContent.AppendLine();
                expandedContent.AppendLine("Haber özeti yukarıda verilmiştir. Tam metin için lütfen kaynak linke tıklayınız.");
            }

            return expandedContent.ToString();
        }

        // RSS'den haber içeriği alma (opsiyonel - RssItem'da Content özelliği yoksa kaldırılabilir)
        private async Task<Models.RssItem?> GetNewsFromRss(int id)
        {
            try
            {
                // Bu metod RssItem'da Content özelliği olmadığı için şu an kullanılmıyor
                // Gelecekte RSS'den daha detaylı bilgi almak için kullanılabilir
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetNewsFromRss metodunda hata");
                return null;
            }
        }

        private async Task<string> ExtractNewsContent(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "tr-TR,tr;q=0.9,en;q=0.8");

                // Timeout ekle
                httpClient.Timeout = TimeSpan.FromSeconds(15);

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return "İçerik yüklenemedi.";
                }

                // Content-Type kontrolü yap - daha esnek hale getir
                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (contentType != null &&
                    !contentType.Contains("text/html") &&
                    !contentType.Contains("text/plain") &&
                    !contentType.Contains("application/xml") &&
                    !contentType.Contains("application/rss"))
                {
                    return "Bu sayfa metin formatında değil.";
                }

                var html = await response.Content.ReadAsStringAsync();

                // Boş içerik kontrolü
                if (string.IsNullOrWhiteSpace(html))
                {
                    return "Sayfa içeriği boş.";
                }

                // Ana makale içeriğini bulmaya çalış
                var articleContent = ExtractMainArticleContent(html);

                if (!string.IsNullOrEmpty(articleContent))
                {
                    return articleContent;
                }

                // Ana içerik bulunamazsa genel temizlik yap
                var content = ExtractTextFromHtml(html);
                return string.IsNullOrEmpty(content) ? "İçerik bulunamadı." : content;
            }
            catch (TaskCanceledException)
            {
                return "İstek zaman aşımına uğradı.";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP hatası: {url}");
                return "Sayfaya erişilemiyor.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"İçerik çekme hatası: {url}");
                return "İçerik yüklenirken hata oluştu.";
            }
        }

        private string ExtractMainArticleContent(string html)
        {
            // Yaygın makale container'larını ara
            var articlePatterns = new[]
            {
                @"<article[^>]*>(.*?)</article>",
                @"<div[^>]*class[^>]*['""][^'""]*article[^'""]*['""][^>]*>(.*?)</div>",
                @"<div[^>]*class[^>]*['""][^'""]*content[^'""]*['""][^>]*>(.*?)</div>",
                @"<div[^>]*class[^>]*['""][^'""]*post[^'""]*['""][^>]*>(.*?)</div>",
                @"<div[^>]*id[^>]*['""][^'""]*content[^'""]*['""][^>]*>(.*?)</div>",
                @"<main[^>]*>(.*?)</main>"
            };

            foreach (var pattern in articlePatterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(html, pattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Singleline);

                if (match.Success)
                {
                    var articleHtml = match.Groups[1].Value;
                    var cleanText = ExtractTextFromHtml($"<div>{articleHtml}</div>");

                    if (!string.IsNullOrEmpty(cleanText) && cleanText.Length > 100)
                    {
                        return cleanText;
                    }
                }
            }

            return string.Empty;
        }

        private string ExtractTextFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "İçerik bulunamadı.";

            try
            {
                // Binary data ve JFIF başlıklarını temizle
                html = System.Text.RegularExpressions.Regex.Replace(html, @"����JFIF.*?��", "",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

                // Base64 ve binary verileri temizle
                html = System.Text.RegularExpressions.Regex.Replace(html, @"[^\x20-\x7E\u00A0-\uFFFF]", " ");

                // Script ve style taglarını kaldır
                html = System.Text.RegularExpressions.Regex.Replace(html, @"<script[^>]*>.*?</script>", "",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                html = System.Text.RegularExpressions.Regex.Replace(html, @"<style[^>]*>.*?</style>", "",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

                // HTML taglarını kaldır
                html = System.Text.RegularExpressions.Regex.Replace(html, @"<[^>]+>", " ");

                // HTML entity'lerini decode et
                html = System.Net.WebUtility.HtmlDecode(html);

                // Fazla boşlukları ve özel karakterleri temizle
                html = System.Text.RegularExpressions.Regex.Replace(html, @"\s+", " ");
                html = html.Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("&quot;", "\"");

                // Başlangıç ve sonundaki boşlukları temizle
                html = html.Trim();

                // Çok kısa içerikleri kontrol et
                if (html.Length < 50)
                {
                    return "İçerik çok kısa veya uygun formatta değil.";
                }

                // İlk 1000 karakteri al
                return html.Length > 1000 ? html.Substring(0, 1000) + "..." : html;
            }
            catch (Exception)
            {
                return "İçerik işlenirken hata oluştu.";
            }
        }
    }
}