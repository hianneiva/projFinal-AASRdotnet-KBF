using KnowledgeBaseForum.API.JWT;
using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using KnowledgeBaseForum.DataAccessLayer.Model;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDao dao;
        private readonly IConfiguration config;

        public AuthController(KbfContext context, IConfiguration config)
        {
            dao = new UsuarioDao(context);
            this.config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginData)
        {
            if (loginData == null)
            {
                return BadRequest(new JwtTokenResponse() { Result = false, Message = "Invalid user request." });
            }

            Usuario? loginUser = await dao.Get(loginData.Username);

            if (loginUser == null || !(loginUser?.Senha?.Equals(loginData.Password)).GetValueOrDefault())
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "Username or password are incorrect." });
            }
            else if (!(loginUser?.Status).GetValueOrDefault())
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "User is deactivated." });
            }

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[Constants.JWT_SECRET]));
            SigningCredentials credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenData = new JwtSecurityToken(
                issuer: config[Constants.JWT_VALID_ISSUER],
                audience: config[Constants.JWT_VALID_AUDIENCE],
                claims: new List<Claim>() 
                {
                    new Claim(ClaimTypes.Role, ClaimRoleNames.USER_ROLE_NAMES[loginUser!.Perfil]),
                    new Claim(ClaimTypes.Email, loginUser.Email),
                    new Claim(ClaimTypes.GivenName, loginUser.Nome),
                    new Claim(ClaimTypes.Name, loginUser.Login)
                },
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenData);

            return Ok(new JwtTokenResponse() { Result = true, Token = token });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(Usuario? usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario?.Nome) || string.IsNullOrEmpty(usuario?.Login) || string.IsNullOrEmpty(usuario?.Senha) ||
                string.IsNullOrEmpty(usuario?.Email))
            {
                return BadRequest(new { result = false, message = "Invalid user data sent." });
            }

            usuario.Status = true;
            usuario.Perfil = 1;
            usuario.UsuarioCriacao = usuario.Login;

            await dao.Add(usuario);

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[Constants.JWT_SECRET]));
            SigningCredentials credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenData = new JwtSecurityToken(
                issuer: config[Constants.JWT_VALID_ISSUER],
                audience: config[Constants.JWT_VALID_AUDIENCE],
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, ClaimRoleNames.USER_ROLE_NAMES[1]),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.GivenName, usuario.Nome),
                    new Claim(ClaimTypes.Name, usuario.Login)
                },
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenData);

            return Created(new Uri(Request.GetEncodedUrl()), new JwtTokenResponse() { Result = true, Token = token });
        }
    }
}
