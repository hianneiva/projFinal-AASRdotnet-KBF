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
    public class ComentarioController : Controller
    {
        private readonly ComentarioDao dao;

        public ComentarioController(KbfContext context)
        {
            dao = new ComentarioDao(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Comentario>> ReadAll() => await dao.All();

        [HttpGet("{id}")]
        public async Task<Comentario?> Read(Guid id) => await dao.Get(id);

        [HttpPost]
        public async Task<IActionResult> Create(Comentario entry)
        {
            try
            {
                await dao.Add(entry);
                return Created(new Uri(Request.GetEncodedUrl()), entry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Comentario entry)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Comentario? found = await dao.Get(id);

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

        [HttpPost("{topicId}")]
        public async Task<IEnumerable<Comentario>> GetAllForTopic(Guid topicId)
        {
            List<Comentario> all = new List<Comentario>();
            all.AddRange(await dao.All());

            return all.Where(c => c.TopicoId == topicId);
        }
    }
}