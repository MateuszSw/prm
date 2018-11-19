using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Prm.API.Models {
    public class User : IdentityUser<int> {

        public User () {
            Articles = new List<Article> ();
            ArticleStudents = new List<ArticleStudent> ();
        
        }

        public string Status { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Article> Articles { get; set; }

        public ICollection<ArticleStudent> ArticleStudents { get; set; }
        public ICollection<UserRole> RolesUser { get; set; }
    }
}