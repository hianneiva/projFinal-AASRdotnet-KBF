using KnowledgeBaseForum.API.Model;
using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDao dao;
        private readonly string passwordCypher;

        public UsuarioController(KbfContext context, IConfiguration config)
        {
            dao = new UsuarioDao(context);
            passwordCypher = config[Constants.CYPHER_SECRET];
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IEnumerable<Usuario>> ReadAll() => await dao.All();

        [HttpGet("{login}")]
        [Authorize(Roles = "ADMIN,NORMAL")]
        public async Task<Usuario?> Read(string login)
        {
            Usuario? user = await dao.Get(login);

            if (user != null)
            {
                user.Senha = null;
            }

            return user;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN,NORMAL")]
        public async Task<IActionResult> Update(UpdateUser payload)
        {
            try
            {
                if (payload.Entry == null)
                {
                    throw new KeyNotFoundException(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                Usuario? user = await dao.Get(payload.Entry!.Login);
                VerifyPassword(payload!.Password!, out bool verified);

                if (!verified)
                {
                    throw new Exception("Cliente não está configurado para acessar o servidor.");
                }
                else if (payload!.Password!.Equals(user!.Senha))
                {
                    throw new Exception("A senha não confere.");
                }

                user!.Email = !string.IsNullOrEmpty(payload.Entry.Email) ? payload.Entry.Email : user.Email;
                user!.Senha = !string.IsNullOrEmpty(payload.Entry.Senha) ? payload.Entry.Senha : user.Senha;
                user.DataModificacao = DateTime.Now;
                user.UsuarioModificacao = payload.Entry.Login;

                await dao.Update(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{login}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Deactivate(string login)
        {
            try
            {
                Usuario? found = await dao.Get(login);

                if (found == null)
                {
                    return NotFound(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                found.Status = false;
                await dao.Update(found);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{login}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Activate(string login)
        {
            try
            {
                Usuario? found = await dao.Get(login);

                if (found == null)
                {
                    return NotFound(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                found.Status = true;
                await dao.Update(found);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
