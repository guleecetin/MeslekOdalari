
namespace MeslekOdalariWebUI.Helpers
{
    public static class SimpleContentCleaner
    {

        public static string CleanNewsContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return "İçerik yüklenemedi.";

            // İlk önce gerçek metin kısmını bul
            // Genelde ilk paragraf temiz olur, sonra binary data başlar

            // JPEG başlangıcını bul (����JFIF veya ��)
            int jpegStart = content.IndexOf("����");
            if (jpegStart == -1)
                jpegStart = content.IndexOf("��");

            // Eğer JPEG verisi varsa, ondan önceki kısmı al
            if (jpegStart > 0)
            {
                content = content.Substring(0, jpegStart);
            }

            // Son 3 nokta varsa onları temizle (...) 
            content = content.TrimEnd('.', ' ');

            // Çok kısa kalmışsa
            if (content.Length < 50)
            {
                return "İçerik tam olarak yüklenemedi. Lütfen orijinal haberi okuyun.";
            }

            // Temizle ve düzenle
            content = content.Trim();

            // Son cümleyi tam bitir
            if (!content.EndsWith(".") && !content.EndsWith("!") && !content.EndsWith("?"))
            {
                // Son kelimeyi bul ve cümleyi düzgün bitir
                int lastSpace = content.LastIndexOf(' ');
                if (lastSpace > content.Length - 20) // Son kelime çok kısaysa
                {
                    content = content.Substring(0, lastSpace) + "...";
                }
                else
                {
                    content += ".";
                }
            }

            return content;
        }

        public static bool IsContentCorrupted(string content)
        {
            if (string.IsNullOrEmpty(content))
                return true;

            // Binary data göstergeleri
            return content.Contains("����") ||
                   content.Contains("��") ||
                   content.Contains("JFIF") ||
                   (content.Length > 100 && content.Count(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) > content.Length * 0.4);
        }

        internal static string CleanNewsContent(object content)
        {
            throw new NotImplementedException();
        }
    }
}