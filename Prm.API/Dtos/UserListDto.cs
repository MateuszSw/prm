using System;

namespace Prm.API.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string PhotoUrl { get; set; }
    }
}