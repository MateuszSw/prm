using System;

namespace Prm.API.Dtos
{
    public class ArticleForCreationEdition
    {
        public int Id { get; set; } 
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Test { get; set; }
      
    }
}