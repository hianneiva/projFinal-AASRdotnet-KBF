using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static KnowledgeBaseForum.AdminWebApp.Utils.Utils;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class UsuarioController : Controller
    {
        private readonly ApiOptions options;
        private readonly IHttpClientFactory factory;

        public UsuarioController(IConfiguration config, IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.options = options.Value;
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
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<IEnumerable<UsuarioViewModel>, object> httpGetter = new HttpHelper<IEnumerable<UsuarioViewModel>, object>(factory, options.ApiHost, token);
                IEnumerable<UsuarioViewModel> queried = await httpGetter.Get(options.ApiUsuario) ?? new List<UsuarioViewModel>();

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
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<UsuarioViewModel, object> httpGetter = new HttpHelper<UsuarioViewModel, object>(factory, options.ApiHost, token);
                UsuarioViewModel? usuario = await httpGetter.Get($"{options.ApiUsuario}/{id}");

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
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<UsuarioViewModel, UsuarioViewModel> httpPutter = new HttpHelper<UsuarioViewModel, UsuarioViewModel>(factory, options.ApiHost, token);
                UsuarioViewModel? editedUsuario = await httpPutter.Put(options.ApiUsuario, usuario);

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
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<bool, object> httpHelper = new HttpHelper<bool, object>(factory, options.ApiHost, token);
                result = status ? await httpHelper.Delete($"{options.ApiUsuario}/{id}") : await httpHelper.Put($"{options.ApiUsuario}/{id}", new { });

                return Json(new { result });
            }
            catch
            {
                throw;
            }
        }
    }
}
