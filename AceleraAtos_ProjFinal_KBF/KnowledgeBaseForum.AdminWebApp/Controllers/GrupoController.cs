using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using static KnowledgeBaseForum.AdminWebApp.Utils.Utils;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class GrupoController : Controller
    {
        private string apiHost;
        private string apiGrupos;
        private IHttpClientFactory factory;

        public GrupoController(IConfiguration config, IHttpClientFactory factory)
        {
            apiHost = config[Constants.API_HOST];
            apiGrupos = config[Constants.API_GRUPOS];
            this.factory = factory;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(string filter)
        {
            try
            {
                HttpHelper<IEnumerable<GrupoViewModel>, object> httpGetter = new HttpHelper<IEnumerable<GrupoViewModel>, object>(factory, apiHost);
                IEnumerable<GrupoViewModel> queried = await httpGetter.Get(apiGrupos) ?? new List<GrupoViewModel>();

                return PartialView("_ListGroups", queried.Where(g => string.IsNullOrEmpty(filter) || 
                                                                    g.Descricao.InvariantContains(filter)));                                                    
            }
            catch 
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                HttpHelper<GrupoViewModel, object> httpGetter = new HttpHelper<GrupoViewModel, object>(factory, apiHost);
                GrupoViewModel grupo = await httpGetter.Get($"{apiGrupos}/{id}");

                return View(grupo);
            }
            catch 
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GrupoViewModel grupo)
        {
            try
            {
                HttpHelper<GrupoViewModel, GrupoViewModel> httpPutter = new HttpHelper<GrupoViewModel, GrupoViewModel>(factory, apiHost);
                GrupoViewModel editedGrupo = await httpPutter.Put(apiGrupos, grupo);

                return RedirectToAction("Index");
            }
            catch 
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ToggleActivation(string id, bool status)
        {
            try
            {
                bool result = true;
                if (status)
                {
                    HttpHelper<bool, object> httpPutter = new HttpHelper<bool, object>(factory, apiHost);
                    result = await httpPutter.Delete($"{apiGrupos}/{id}");
                }
                else
                {
                    HttpHelper<bool, object> httpPutter = new HttpHelper<bool, object>(factory, apiHost);
                    result = await httpPutter.Put($"{apiGrupos}/{id}", new { });
                }

                return Json(new { result });
            }
            catch 
            {

                throw;
            }
        }   
    }
}
