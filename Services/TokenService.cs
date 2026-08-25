using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace CatalogoApi.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            //Obtendo a chave secreta definida no arquio appsettings.json
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                      throw new InvalidOperationException("Invalid secret Key");

            //Convertendo a chave secreta em bytes
            var privateKey = Encoding.UTF8.GetBytes(key);

            //Criando a assinatura digital do token
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                    SecurityAlgorithms.HmacSha256);

            //Criando o descritor do token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Definindo os valores das claims
                Subject = new ClaimsIdentity(claims),

                //Data de expiração do token
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT")
                                    .GetValue<double>("TokenValidityInMinutes")),

                //Defininfo a audiencia
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),

                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            //Obtendo a chave secreta 
            var secrectKey = _config["JWT: SecretKey"] ?? throw new InvalidOperationException("Invalid secret Key");

            //Validações do token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey)),
                ValidateLifetime = false, //Aqui estamos desabilitando a validação de expiração do token

            };

            //Manipulando o token
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                       out SecurityToken securityToken);
            //Nova validação do token 
            if(securityToken is not JwtSecurityToken jwtSecurityToken || 
                                                     jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                     StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}
