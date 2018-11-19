using System;
using System.Collections.Generic;
using Prm.API.Models;

namespace Prm.API.Dtos
{
    public class UserDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public int Age { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<UserRole> RolesUser { get; set; }
        public ICollection<PhotosDetailedDto> Photos { get; set; }

        public ICollection<ArticleForListDto> CreatedArticles { get; set; }

        public ICollection<ArticleForListDto> SubscribedArticles { get; set; }
    }
}