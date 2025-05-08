using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.RegulationDtos
{
    public class UpdateRegulationDto
    {
        public ObjectId Id { get; set; }
        public string HistoryofTradesmenandCraftsmenLawUrl { get; set; }
        public string TradesmenArtisansLawNo5362Url { get; set; }
        public string RegulationsUrl { get; set; }
        public string CircularsUrl { get; set; }
        public string NotificationsUrl { get; set; }
    }
}
