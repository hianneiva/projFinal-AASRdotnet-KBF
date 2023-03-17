using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class TagController : Controller
    {
        private readonly ApiOptions options;
        private readonly IHttpClientFactory factory;

        public TagController(IConfiguration config, IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> List(string filter)
        {
            string? token = this.Request.Cookies.GetTokenFromCookies();
            IEnumerable<TagViewModel>? tagList = await new HttpHelper<IEnumerable<TagViewModel>, object>(factory, options.ApiHost, token).Get(options.ApiTags);

            return PartialView("_ListTags", tagList?.Where(t => string.IsNullOrEmpty(filter) || t.Descricao.InvariantContains(filter)) ?? new List<TagViewModel>());
        }

        [HttpPost]
        public async Task<JsonResult> Edit(Guid id, string name)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<TagViewModel, TagViewModel> httpHelper = new HttpHelper<TagViewModel, TagViewModel>(factory, options.ApiHost, token);
                TagViewModel? toEdit = await httpHelper.Get($"{options.ApiTags}/{id}");

                if (toEdit == null)
                {
                    return Json(new { result = false, message = "Tag não foi encontrada" });
                }

                toEdit.Descricao = name;
                TagViewModel? result = await httpHelper.Put(options.ApiTags, toEdit);

                if (result != null)
                {
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = true, message = "Falha na atualização do registro." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Falha não esperada: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<object, object> httpHelper = new HttpHelper<object, object>(factory, options.ApiHost, token);
                await httpHelper.Delete($"{options.ApiRelationalTopicoTag}/{id}");
                await httpHelper.Delete($"{options.ApiTags}/{id}");

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Falha não esperada: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                string username = this.User.Claims.First(c => c.Type.Equals(ClaimTypes.Name)).Value;
                TagViewModel toCreate = new()
                {
                    Descricao = name,
                    UsuarioCriacao = username
                };
                TagViewModel? created = await new HttpHelper<TagViewModel, object>(factory, options.ApiHost, token).Post(options.ApiTags, toCreate);

                if (created == null)
                {
                    return Json(new { result = false, message = $"Falha na criação da Tag." });
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = $"Falha não esperada: {ex.Message}" });
            }
        }
    }
}
