using KnowledgeBaseForum.DataAccessLayer.Repository.Impl;
using KnowledgeBaseForum.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KnowledgeBaseForum.DataAccessLayer.Repository.Impl.Association;
using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;

namespace KnowledgeBaseForum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationalController : ControllerBase
    {
        private readonly TopicoTagDao ttDao;
        private readonly UsuarioGrupoDao ugDao;

        public RelationalController(KbfContext context)
        {
            ttDao = new TopicoTagDao(context);
            ugDao = new UsuarioGrupoDao(context);
        }

        [HttpPost("topicoTag")]
        public async Task<string> CreateTTRelation(Guid tagId, Guid topicId)
        {
            try
            {
                await ttDao.Add(topicId, tagId);
                return "true";
            }
            catch (Exception ex)
            {
                return $"Failure: {ex.Message}";
            }
        }
        
        [HttpDelete("topicoTag")]
        public async Task<string> DeleteTTRelation(Guid tagId, Guid topicId)
        {
            try
            {
                await ttDao.Delete(topicId, tagId);
                return "true";
            }
            catch (Exception ex)
            {
                return $"Failure: {ex.Message}";
            }
        }

        [HttpDelete("topicoTag/{tagId}")]
        public async Task<string> DeleteAllTagsRelation(Guid tagId)
        {
            try
            {
                await ttDao.DeleteTagAssociations(tagId);
                return "true";
            }
            catch (Exception ex)
            {
                return $"Failure: {ex.Message}";
            }
        }

        [HttpPost("usuarioGrupo")]
        public async Task<string> CreateUGRelation(string login, Guid groupId)
        {
            try
            {
                await ugDao.Add(login, groupId);
                return "true";
            }
            catch (Exception ex)
            {
                return $"Failure: {ex.Message}";
            }
        }

        [HttpDelete("usuarioGrupo")]
        public async Task<string> DeleteUGRelation(string login, Guid groupId)
        {
            try
            {
                await ugDao.Delete(login, groupId);
                return "true";
            }
            catch (Exception ex)
            {
                return $"Failure: {ex.Message}";
            }
        }
    }
}
