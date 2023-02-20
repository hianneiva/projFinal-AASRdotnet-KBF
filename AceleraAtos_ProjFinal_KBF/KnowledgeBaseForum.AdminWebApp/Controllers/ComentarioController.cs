using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ComentarioController : Controller
    {
        private readonly ApiOptions options;
        private readonly IHttpClientFactory factory;

        public ComentarioController(IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> ListforTopic(Guid topicId)
        {
                    string? token = this.Request.Cookies.GetTokenFromCookies();
            HttpHelper<IEnumerable<ComentarioViewModel>, object> httpPoster = new HttpHelper<IEnumerable<ComentarioViewModel>, object>(factory, options.ApiHost, token);
            IEnumerable<ComentarioViewModel>? commentsForTopic = await httpPoster.Post($"{options.ApiComentario}/{topicId}", new { });

            return PartialView("_ListComments", commentsForTopic ?? new List<ComentarioViewModel>());
        }

        [HttpPost]
        public async Task<JsonResult> ToggleStatus(Guid id)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<ComentarioViewModel, ComentarioViewModel> httpHelper = new HttpHelper<ComentarioViewModel, ComentarioViewModel>(factory, options.ApiHost, token);
                ComentarioViewModel? original = await httpHelper.Get($"{options.ApiComentario}/{id}");

                if (original == null)
                {
                    return Json(new { result = false, message = $"Comentário não encontrado" });
                }

                original.Status = !original.Status;
                ComentarioViewModel? updated = await httpHelper.Put(options.ApiComentario, original);

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
