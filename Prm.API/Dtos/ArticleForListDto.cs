using System;

namespace Prm.API.Dtos
{
    public class ArticleForListDto
    {
        public int Id { get; set; }
        public UserListDto Author { get; set; }
        public string Title { get; set; }
    }
}