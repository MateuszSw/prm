using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prm.API.Models
{
    public class ArticleStudent
    {        
        
        public int ArticleId {get; set;}
        public Article Article { get; set; }        

        
        public int StudentId {get; set;}
        public User Student { get; set; }
                
    }
}