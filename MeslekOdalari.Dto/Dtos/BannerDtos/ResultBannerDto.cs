﻿using MongoDB.Bson;

namespace MeslekOdalari.Dto.Dtos.BannerDtos
{
   public class ResultBannerDto
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
