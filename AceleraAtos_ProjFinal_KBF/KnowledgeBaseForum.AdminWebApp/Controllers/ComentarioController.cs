using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class ComentarioController : Controller
    {
        private readonly string apiHost;
        private readonly string apiComentario;
        private readonly IHttpClientFactory factory;

        public ComentarioController(IConfiguration config, IHttpClientFactory factory)
        {
            this.factory = factory;
            apiHost = config[Constants.API_HOST];
            apiComentario = config[Constants.API_COMENTARIO];
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> ListforTopic(Guid topicId)
        {
            HttpHelper<IEnumerable<ComentarioViewModel>, object> httpPoster = new HttpHelper<IEnumerable<ComentarioViewModel>, object>(factory, apiHost);
            IEnumerable<ComentarioViewModel>? commentsForTopic = await httpPoster.Post($"{apiComentario}/{topicId}", new { });

            return PartialView("_ListComments", commentsForTopic ?? new List<ComentarioViewModel>());
        }

        [HttpPost]
        public async Task<JsonResult> ToggleStatus(Guid id)
        {
            try 
            {
                HttpHelper<ComentarioViewModel, ComentarioViewModel> httpHelper = new HttpHelper<ComentarioViewModel, ComentarioViewModel>(factory, apiHost);
                ComentarioViewModel? original = await httpHelper.Get($"{apiComentario}/{id}");

                if (original == null)
                {
                    return Json(new { result = false, message = $"Comentário não encontrado" });
                }

                original.Status = !original.Status;
                ComentarioViewModel? updated = await httpHelper.Put(apiComentario, original);

                if (updated == null)
                {
                    return Json(new { result = false, message = $"Comentário não atualizado" });
                }

                return Json(new { result = true });
            }
            catch (Exception ex) 
            {
                return Json(new { result = false, message = $"Falha não esperada - Detalhes: {ex.Message}" });
            }
        }
    }
}
