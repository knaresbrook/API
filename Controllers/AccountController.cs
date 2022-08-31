using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountController(
            DataContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _config = config;
            _roleManager = roleManager;
        }

        [HttpGet("GetUsers")]
        public IEnumerable<IdentityUser> GetUsers()
        {
            IEnumerable<IdentityUser> users = _userManager.Users.ToList();
            return users;
        }

        [HttpGet("GetRole")]
        public async Task<string> GetRole(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult> CreateRole(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("DeleteRole")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null) return BadRequest("Role not found!");
            else
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest(result.Errors);
            }
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(RoleDto roleDto)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(roleDto.RoleName);
            if (role == null) return BadRequest("Invalid Role");

            IdentityUser user = await _userManager.FindByNameAsync(roleDto.UserName);
            if (user == null) return BadRequest("Invalid User!");

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserTokenDto>> Register(UserDTO userDto)
        {
            if (userDto == null) return BadRequest("Invalid User");

            var user = new IdentityUser
            {
                UserName = userDto.Email,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                IdentityUser _user = await _userManager.FindByNameAsync(userDto.Email);
                if (_user == null) return BadRequest("Invalid User!");


                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                {
                    return new UserTokenDto
                    {
                        userName = user.UserName,
                        token = GetToken(user)
                    };
                }
                else
                    return BadRequest(roleResult.Errors);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login(UserDTO userDto)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(userDto.Email, userDto.Password, false, false);

            if (!loginResult.Succeeded)
            {
                return BadRequest("Invalid Username/Password...");
            }

            var user = await _userManager.FindByNameAsync(userDto.Email);

            return new UserTokenDto
            {
                userName = user.UserName,
                token = GetToken(user)
            };
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private String GetToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")); //this._config.GetValue<String>("THIS IS MY KEY")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(1000), //this._config.GetValue<int>("Tokens:Lifetime")),
                audience: "test",//this._config.GetValue<String>("Tokens:Audience"),
                issuer: "test"//this._config.GetValue<String>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}