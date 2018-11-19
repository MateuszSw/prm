using System;

namespace Prm.API.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int IdSender { get; set; }
        public string SenderSurname { get; set; }
        public string SenderImage { get; set; }
        public int RecipientId { get; set; }
        public string RecipientSurname { get; set; }
        public string RecipientImage { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}