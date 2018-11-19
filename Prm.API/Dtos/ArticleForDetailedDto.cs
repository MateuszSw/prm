using System;

namespace Prm.API.Dtos
{
    public class ArticleForDetailesDto
    {
        public int Id { get; set; }
        public UserListDto Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Test { get; set; }
        
        public QuestionDto[] Questions  { get; set; }

        public UserListDto[] Students { get; set; }

    }
}