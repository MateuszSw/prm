using System.Collections.Generic;

namespace Prm.API.Models
{
    public class Article
    {

        public Article()
        {
            Students = new List<ArticleStudent>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId{get; set;}
        public User Author { get; set; }

        public string Test { get; set; }

        public ICollection<ArticleStudent> Students {get; set;}
    }
}