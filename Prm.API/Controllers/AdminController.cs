using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prm.API.Data;
using Prm.API.Dtos;
using Prm.API.Helpers;
using Prm.API.Models;

namespace Prm.API.Controllers {
    [ApiController]
    [Route ("api/[controller]")]
    public class AdminController : ControllerBase {
        private readonly DataContext _context;
        private readonly UserManager<User> _user;
        private readonly IOptions<CloudinaryDotNet.Cloudinary> _cloudinaryConfig;
        private CloudinaryDotNet.Cloudinary _cloudinary;

        public AdminController (
            DataContext context,
            UserManager<User> userManager,
            IOptions<CloudinaryDotNet.Cloudinary> cloudinaryConfig) {
            _user = userManager;
            _cloudinaryConfig = cloudinaryConfig;
            _context = context;

            
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet ("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles () {
            var userList = await (from user in _context.Users orderby user.UserName select new {
                Id = user.Id,
                    UserName = user.UserName,
                    Roles = (from userRole in user.RolesUser join role in _context.Roles on userRole.RoleId equals role.Id select role.Name).ToList ()
            }).ToListAsync ();
            return Ok (userList);
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost ("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles (string userName, RoleEditDto roleEditDto) {
            var user = await _user.FindByNameAsync (userName);

            var userRoles = await _user.GetRolesAsync (user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };
            var score = await _user.AddToRolesAsync (user, selectedRoles.Except (userRoles));

            if (!score.Succeeded)
                return BadRequest ("Failed to add to roles");

            score = await _user.RemoveFromRolesAsync (user, userRoles.Except (selectedRoles));

            if (!score.Succeeded)
                return BadRequest ("Failed to remove the roles");

            return Ok (await _user.GetRolesAsync (user));
        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpPost ("deleteUser/{userName}")]
        public async Task<IActionResult> DeleteUser (string userName) {
            if (User.Identity.Name.ToUpper () == userName) {
                return BadRequest ("Failed to remove currently logged user");
            }

            var user = await _context.Users
                .Include (u => u.Photos)
                .Include (u => u.Articles).ThenInclude(a => a.Students)
                .Include (u => u.ArticleStudents)
                .FirstOrDefaultAsync (u => u.UserName == userName);

            if (user == null) {
                return BadRequest ("Failed to remove non existing user");
            }

            foreach (var photo in user.Photos) {
                _context.Remove (photo);
            }

            foreach (var articleStudent in user.ArticleStudents) {
                _context.Remove (articleStudent);
            }

            foreach (var article in user.Articles) {
                foreach (var articleStudent in article.Students) {
                _context.Remove (articleStudent);
            }
                _context.Remove (article);
            }

            await _context.SaveChangesAsync ();

            var score = await _user.DeleteAsync (user);

            if (!score.Succeeded)
                return BadRequest ("Failed to add to roles");

            return Ok ();
        }
    }
}