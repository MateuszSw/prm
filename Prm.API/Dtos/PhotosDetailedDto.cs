using System;

namespace Prm.API.Dtos
{
    public class PhotosDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime TimeAdd { get; set; }
        public bool Main { get; set; }
        public bool Approved { get; set; }
    }
}