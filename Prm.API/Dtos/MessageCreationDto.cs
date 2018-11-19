using System;

namespace Prm.API.Dtos
{
    public class MessageCreationDto
    {
        public int IdSender { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}