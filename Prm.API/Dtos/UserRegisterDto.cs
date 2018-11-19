using System;
using System.ComponentModel.DataAnnotations;

namespace Prm.API.Dtos
{
    public class UserRegisterDtoDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 3, ErrorMessage = "hasło musi mieć minimum 3 znaki maksimum 9")]
        public string Password { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string City { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserRegisterDtoDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}