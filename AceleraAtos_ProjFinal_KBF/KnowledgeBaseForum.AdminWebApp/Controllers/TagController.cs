using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly string apiHost;
        private readonly string apiTags;
        private readonly string apiRelTopicoTags;
        private readonly IHttpClientFactory factory;

        public TagController(IConfiguration config, IHttpClientFactory factory)
        {
            this.factory = factory;
            apiHost = config[Constants.API_HOST];
            apiTags = config[Constants.API_TAGS];
            apiRelTopicoTags = config[Constants.API_REL_TOPICO_TAG];
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> List(string filter)
        {
            IEnumerable<TagViewModel>? tagList = await new HttpHelper<IEnumerable<TagViewModel>, object>(factory, apiHost).Get(apiTags);

            return PartialView("_ListTags", tagList?.Where(t => string.IsNullOrEmpty(filter) || t.Descricao.InvariantContains(filter)) ?? new List<TagViewModel>());
        }

        [HttpPost]
        public async Task<JsonResult> Edit(Guid id, string name)
        {
            try
            {
                HttpHelper<TagViewModel, TagViewModel> httpHelper = new HttpHelper<TagViewModel, TagViewModel>(factory, apiHost);
                TagViewModel? toEdit = await httpHelper.Get($"{apiTags}/{id}");

                if (toEdit == null)
                {
                    return Json(new { result = false, message = "Tag não foi encontrada" });
                }
                
                toEdit.Descricao = name;
                TagViewModel? result = await httpHelper.Put(apiTags, toEdit);

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
                HttpHelper<object, object> httpHelper = new HttpHelper<object, object>(factory, apiHost);
                await httpHelper.Delete($"{apiRelTopicoTags}/{id}");
                await httpHelper.Delete($"{apiTags}/{id}");

                return Json(new {  result = true });
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
                TagViewModel toCreate = new TagViewModel()
                {
                    Descricao = name,
                    UsuarioCriacao = "SYSTEM"
                };
                TagViewModel? created = await new HttpHelper<TagViewModel, object>(factory, apiHost).Post(apiTags, toCreate);

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
