using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ElementscrAPI.Filters
{
    public class JwtAuth : IJwtAuth
    {
        private readonly IConfiguration _config;
        
        public JwtAuth(IConfiguration config)
        {
            _config = config;
        }
        
        public string Authentication(string username, string accountId, string isAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, "ElementsRevival"),
                new("username", username),
                new("accountId", accountId),
            };
            if (isAdmin == "yes")
            {
                claims.Add(
                    new("admin", "yes"));
            }
            
            var timeSpan = TimeSpan.FromMinutes(15);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(timeSpan),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var secToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(secToken);
        }
        public string AccessTokenCreation(string username, string accountId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, "ElementsRevival"),
                new("username", username),
                new("accountId", accountId)
            };
            
            var timeSpan = TimeSpan.FromDays(2);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(timeSpan),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var secToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(secToken);
        }
        
        public string GetAccountId(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var accountId = tokenS.Claims.First(claim => claim.Type == "accountId").Value;
            return accountId;
        }


        public string RefreshToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var accountId = tokenS.Claims.First(claim => claim.Type == "accountId").Value;
            var username = tokenS.Claims.First(claim => claim.Type == "username").Value;

            return Authentication(username, accountId, "no");
        }
    }
}
