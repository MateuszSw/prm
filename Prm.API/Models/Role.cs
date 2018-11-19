using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Prm.API.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> RolesUser { get; set; }
    }
}