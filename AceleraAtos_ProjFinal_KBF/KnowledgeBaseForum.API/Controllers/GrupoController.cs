using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : Controller
    {        
        private GrupoDao dao;

        public GrupoController(KbfContext context)
        {
            dao = new GrupoDao(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Grupo>> ReadAll() => await dao.All();

        [HttpGet("{grupo}")]
        public async Task<Grupo?> Read(Guid grupo) => await dao.Get(grupo);

        [HttpPost]
        public async Task<IActionResult> Create(Grupo entry)
        {
            try
            {
                Grupo? entryExist = await dao.GetDescription(entry.Descricao);

                if (entryExist == null)
                {
                    await dao.Add(entry);
                    return Created(new Uri(Request.GetEncodedUrl()), entry);
                }
                else
                {
                    entryExist.Status = true;
                    await dao.Update(entryExist);
                    return Ok(entryExist);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{entry}")]
        public async Task<IActionResult> Update(Grupo entry)
        {
            try
            {
                if (entry == null)
                {
                    throw new KeyNotFoundException(Constants.ENTITY_NOT_FOUND_IN_DB);
                }

                await dao.Update(entry);
                return Ok(entry);
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

        [HttpDelete("{entry}")]
        public async Task<IActionResult> Delete(Guid entry)
        {
            try
            {
                Grupo? found = await dao.Get(entry);

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
    }
}