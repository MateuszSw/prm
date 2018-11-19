using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Prm.API.Models;

namespace Prm.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string City { get; set; }


        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<UserRole> RolesUser { get; set; }

        public UserRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}