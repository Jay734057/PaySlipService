using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cw.Platform.Jwt
{
    public class JwtProvider : IJwtProvider
    {
        readonly TokenValidationParameters _parameters;
        readonly Action<SecurityTokenDescriptor> _options;
        readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public JwtProvider(TokenValidationParameters parameters, Action<SecurityTokenDescriptor> options)
        {
            _parameters = parameters;
            _options = options;
        }

        // Generate token method
    }
}
