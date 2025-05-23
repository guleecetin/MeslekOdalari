﻿namespace MeslekOdalari.Dto.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageDate { get; set; } = DateTime.Now;
    }
}
