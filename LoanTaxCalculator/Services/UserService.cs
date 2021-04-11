using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Entities;
using LoanTaxCalculator.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Services
{
    public class UserService
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(IOptions<AppSettings> appSettings, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> AuthenticateAsync(string username, string password)
        {
            return await _signInManager.PasswordSignInAsync(username, password, false, false);
        }

        public async Task<string> GetTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.Concat(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                })),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest registerModel)
        {
            var user = new User { UserName = registerModel.Username };
            return await _userManager.CreateAsync(user, registerModel.Password);
        }

        public async Task UpdateUserRoleAsync(string id, Role role)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role.ToString());
        }
    }
}
