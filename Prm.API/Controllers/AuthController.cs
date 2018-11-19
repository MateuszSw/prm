using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Prm.API.Data;
using Prm.API.Dtos;
using Prm.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Prm.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _user;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _user = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            var userCreate = _mapper.Map<User>(userRegister);
            IdentityResult roleResult;
            var score = await _user.CreateAsync(userCreate, userRegister.Password);
            User newUser = await _user.Users.FirstOrDefaultAsync(x => x.UserName == userRegister.UserName);
            if (newUser.Status == "student")
            {
                roleResult = await _user.AddToRoleAsync(newUser, "Student");
            }
            else{   
                roleResult = await _user.AddToRoleAsync(newUser, "Teacher");
            }
            var userToReturn = _mapper.Map<UserDetailedDto>(userCreate);
            if (score.Succeeded)
            {
                return Ok(userToReturn);
            }

            return BadRequest(score.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _user.FindByNameAsync(userLoginDto.Username);
            var score = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
            if (score.Succeeded)
            {
                var appUser = await _user.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userLoginDto.Username.ToUpper());
                var userToReturn = _mapper.Map<UserListDto>(appUser);
                return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,  user = userToReturn
                });}
            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _user.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}