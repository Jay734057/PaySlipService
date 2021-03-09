using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cw.Platform.Jwt
{
    public class JwtProvider
    {
        private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        // Generate token method
        public static string GenerateJWTToken(int userId, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            //set the claim identity name as user id
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, userId.ToString())
            };

            //setup token with claims, validation period and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return _jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
