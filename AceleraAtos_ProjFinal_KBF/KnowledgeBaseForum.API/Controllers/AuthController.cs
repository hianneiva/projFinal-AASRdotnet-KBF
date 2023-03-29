using KnowledgeBaseForum.API.Model;
using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.Commons.JWT;
using KnowledgeBaseForum.Commons.JWT.Model;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDao dao;
        private readonly ITokenService tokenService;
        private readonly string passwordCypher;

        public AuthController(KbfContext context, ITokenService tokenService, IConfiguration config)
        {
            dao = new UsuarioDao(context);
            this.tokenService = tokenService;
            passwordCypher = config[Constants.CYPHER_SECRET];
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginData)
        {
            if (loginData == null)
            {
                return BadRequest(new JwtTokenResponse() { Result = false, Message = "Credenciais inválidas" });
            }

            Usuario? loginUser = await dao.Get(loginData.Username.ToUpper());
            string semiDecodedPwd = VerifyPassword(loginData.Password, out bool verified);

            if (loginUser == null)
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "Usuário não encontrado" });
            }
            else if (loginUser != null && !(loginUser?.Senha?.Equals(semiDecodedPwd)).GetValueOrDefault())
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "Usuário ou senha incorretos" });
            }
            else if (!verified)
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "Cliente inválido" });
            }
            else if (!(loginUser?.Status).GetValueOrDefault())
            {
                return Unauthorized(new JwtTokenResponse() { Result = false, Message = "Usuário está desativado" });
            }

            UserContainer container = new() { Email = loginUser!.Email, GivenName = loginUser!.Nome, Profile = loginUser!.Perfil, Username = loginUser!.Login };
            string token = tokenService.GenerateToken(container, null);

            return Ok(new JwtTokenResponse() { Result = true, Token = token });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(Usuario? usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario?.Nome) || string.IsNullOrEmpty(usuario?.Login) || string.IsNullOrEmpty(usuario?.Senha) ||
                string.IsNullOrEmpty(usuario?.Email))
            {
                return BadRequest(new { result = false, message = "Dados de novo usuário inválidos ou ausentes" });
            }

            string semiDecodedPwd = VerifyPassword(usuario.Senha, out bool verified);

            if (!verified)
            {
                return Unauthorized(new { result = false, message = "Cliente inválido" });
            }

            usuario.Status = true;
            usuario.Perfil = 1;
            usuario.UsuarioCriacao = usuario.Login;
            usuario.Senha = semiDecodedPwd;
            await dao.Add(usuario);

            UserContainer container = new() { Email = usuario.Email, GivenName = usuario.Nome, Profile = usuario.Perfil, Username = usuario.Login };
            string token = tokenService.GenerateToken(container, null);

            return Created(new Uri(Request.GetEncodedUrl()), new JwtTokenResponse() { Result = true, Token = token });
        }

        private string VerifyPassword(string password, out bool result)
        {
            string encodedSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordCypher));
            string[] passwordParts = password.Split('.');
            result = !string.IsNullOrEmpty(passwordParts.Last()) && encodedSecret.Equals(passwordParts.Last());

            return passwordParts.First();
        }
    }
}
