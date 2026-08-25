using CatalogoApi.API.DTO;
using CatalogoApi.Domain;
using CatalogoApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CatalogoApi.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CatalogoApi.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Serviços para injetar no controlador
        private readonly ITokenService _tokenService;
        private readonly UserManager<AplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        //injetando os serviços no construtor do controlador
        public AuthController(ITokenService tokenService, 
                              UserManager<AplicationUser> userManager, 
                              RoleManager<IdentityRole> roleManager, 
                              IConfiguration configuration,
                              ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //Tentando encontrar o usuário pelo nome de usuário
            var user = await _userManager.FindByNameAsync(model.UserName!);

            //Validando se o usuario for encontrado e a senha for encontrada
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
               //Obtendo os perfis deste usuário
                var userRoles = await _userManager.GetRolesAsync(user);

                //Criando as claims (declarações) de autenticação
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id", user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                //Incluindo os perfis na lista de declarações do claims
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                //Gerando o token
                var token = _tokenService.GenerateAcessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshTTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                //Atualizando o valor do refresh token do user
                user.RefreshToken = refreshToken;

                //Persistir as informações na tabela do banco de dados
                await _userManager.UpdateAsync(user);

                //Retornando o JSON
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            //Verificando se o usuário já existe
            var userExists = await _userManager.FindByNameAsync(model.Username!);

            if(userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new Response { Status = "Error", Message = "User already exists" });
            }

            AplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new Response { Status = "Error", Message = "User creation failed" });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully" });

        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken (TokenModel tokenModel)
        {
            //Verifica se é null
            if(tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            //Extraindo o valor do AcessToken
            string? acessToken = tokenModel.AcessToken ?? 
                                 throw new ArgumentNullException(nameof(tokenModel));

            //Extraindo o valor do RefreshToken
            string? refreshToken = tokenModel.RefreshToken ?? 
                                   throw new ArgumentNullException(nameof(tokenModel));

            //Extraindo as claims 
            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

            //Verificando se o principal é null
            if(principal == null)
            {
                return BadRequest("Invalid access token or refresh token"); 
            }

            //Extraindo o nome do usuário de principal
            string username = principal.Identity.Name;

            //Localizando o usuário no banco de dados
            var user = await _userManager.FindByNameAsync(username!);

            //Verificações do objeto user
            if(user == null || user.RefreshToken != refreshToken 
                            || user.RefreshTTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            //Gerando o novo token de acesso
            var newAccessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _configuration);

            //Gerando o novo refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            return new ObjectResult(new
            {
                acessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize(Policy = "ExclusiveOnly")]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke (string username)
        {
            //Localizando o usuário 
            var user = await _userManager.FindByNameAsync(username);

            //Verifica se é null
            if(user == null) return BadRequest("Invalid user name");

            //Invalidando o refresh token do usuário
            user.RefreshToken = null;

            //Persistindo as alterações no banco de dados
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("CreateRoule")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> CreateRole (string roleName)
        {
            //Tentando localizar a role no banco de dados
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                      return StatusCode(StatusCodes.Status200OK, 
                             new Response { Status = "Success", 
                             Message = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest, 
                             new Response { Status = "Error", 
                             Message = $"Issue adding te new {roleName} role" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, 
                             new Response { Status = "Error", 
                             Message = $"Role {roleName} already exists" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> AddUserToRole (string email, string roleName)
        {
            //Valido o usuário pelo email
            var user = await _userManager.FindByEmailAsync(email);

            if(user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if(result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {email} added to role {roleName} successfully");
                    return StatusCode(StatusCodes.Status200OK, 
                             new Response { Status = "Success", 
                             Message = $"User {email} added to role {roleName} successfully" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role"); 
                    return StatusCode(StatusCodes.Status400BadRequest, 
                             new Response { Status = "Error", 
                             Message = $"Issue adding user {email} to role {roleName}" });
                }
            }
            else
            {
                return BadRequest(new { error = "Unable to find user" });
            }
        }
    }
}
