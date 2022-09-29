using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RozetkaFinder.Models.User;
using RozetkaFinder.Models;

namespace RozetkaFinder.Services.Security.JwtToken
{
    public interface IJwtService
    {
        string GenerateJwtTokenAsync(User user, JwtSalt tokenSalt);
    }

    public class JwtService : IJwtService
    {
        public string GenerateJwtTokenAsync(User user, JwtSalt tokenSalt)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                tokenSalt.Key));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;
        }
    }
}
