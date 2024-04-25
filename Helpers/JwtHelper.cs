using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace Backend.Helpers
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(User user)
        {
            var userData = new
            {
                first_name = user.FirstName,
                last_name = user.LastName,
                gender = user.Gender,
                email = user.Email,
                role = user.Role
            };
            var claims = new[]
            {
                new Claim("data", JsonSerializer.Serialize(userData)),
            };
            var securityKey = GenerateSecureKey();
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "FoodUp",
                audience: "RestaurantApi",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static SecurityKey GenerateSecureKey()
        {
            var key = new byte[256];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return new SymmetricSecurityKey(key);
        }
    }
}
