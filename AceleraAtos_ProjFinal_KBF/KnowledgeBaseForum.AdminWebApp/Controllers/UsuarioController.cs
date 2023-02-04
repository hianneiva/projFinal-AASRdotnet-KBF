using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using static KnowledgeBaseForum.AdminWebApp.Utils.Utils;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string apiHost;
        private readonly string apiUsuarios;
        private readonly IHttpClientFactory factory;

        public UsuarioController(IConfiguration config, IHttpClientFactory factory)
        {
            apiHost = config[Constants.API_HOST];
            apiUsuarios = config[Constants.API_USUARIO];
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
                HttpHelper<IEnumerable<UsuarioViewModel>, object> httpGetter = new HttpHelper<IEnumerable<UsuarioViewModel>, object>(factory, apiHost);
                IEnumerable<UsuarioViewModel> queried = await httpGetter.Get(apiUsuarios) ?? new List<UsuarioViewModel>();

                return PartialView("_ListUsers", queried.Where(u => string.IsNullOrEmpty(filter) ||
                                                                    u.Nome.InvariantContains(filter) ||
                                                                    u.Email.InvariantContains(filter) ||
                                                                    u.Login.InvariantContains(filter)));
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
                HttpHelper<UsuarioViewModel, object> httpGetter = new HttpHelper<UsuarioViewModel, object>(factory, apiHost);
                UsuarioViewModel? usuario = await httpGetter.Get($"{apiUsuarios}/{id}");

                return View(usuario);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioViewModel usuario)
        {
            try
            {
                HttpHelper<UsuarioViewModel, UsuarioViewModel> httpPutter = new HttpHelper<UsuarioViewModel, UsuarioViewModel>(factory, apiHost);
                UsuarioViewModel? editedUsuario = await httpPutter.Put(apiUsuarios, usuario);

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
                HttpHelper<bool, object> httpHelper = new HttpHelper<bool, object>(factory, apiHost);
                result = status ? await httpHelper.Delete($"{apiUsuarios}/{id}") : await httpHelper.Put($"{apiUsuarios}/{id}", new { });

                return Json(new { result });
            }
            catch
            {
                throw;
            }
        }
    }
}
