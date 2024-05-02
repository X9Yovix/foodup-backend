using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Helpers
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(User user)
        {
			/*
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
               */
			var claims = new[]
{
				new Claim("firstName", user.FirstName),
				new Claim("lastName", user.LastName),
				new Claim("email", user.Email),
				new Claim("gender", user.Gender),
				new Claim(ClaimTypes.Role, user.Role)
			};
			//var securityKey = GenerateSecureKey();
			var key = Encoding.UTF8.GetBytes("123456789123456789123456789123456789123456789123456789");

			var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "FoodUp",
                audience: "RestaurantApi",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /*
        private static SecurityKey GenerateSecureKey()
        {
            var key = new byte[256];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return new SymmetricSecurityKey("FoodUp");
        }
        */
    }
}
