using System;

namespace Prm.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime TimeAdd { get; set; }
        public bool Main { get; set; }
        public string IdPublic { get; set; }
        public bool Approved { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}