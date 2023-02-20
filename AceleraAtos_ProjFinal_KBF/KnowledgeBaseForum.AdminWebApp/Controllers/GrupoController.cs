using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using static KnowledgeBaseForum.AdminWebApp.Utils.Utils;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class GrupoController : Controller
    {
        private const string UPDATE_FAILURE_MSG = "Falha na atualização";
        private readonly ApiOptions options;
        private readonly IHttpClientFactory factory;

        public GrupoController(IConfiguration config, IHttpClientFactory factory, IOptions<ApiOptions> options)
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
                HttpHelper<IEnumerable<GrupoViewModel>, object> httpGetter = new HttpHelper<IEnumerable<GrupoViewModel>, object>(factory, options.ApiHost, token);
                IEnumerable<GrupoViewModel> queried = await httpGetter.Get(options.ApiGrupo) ?? new List<GrupoViewModel>();

                return PartialView("_ListGroups", queried.Where(g => string.IsNullOrEmpty(filter) ||
                                                                    g.Descricao.InvariantContains(filter)));
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Create()
        {
            string? token = this.Request.Cookies.GetTokenFromCookies();
            List<UsuarioViewModel> allUsers = await new HttpHelper<List<UsuarioViewModel>, object>(factory, options.ApiHost, token).Get(options.ApiUsuario) ??
                                              new List<UsuarioViewModel>();
            ViewData["Users"] = allUsers.ToDictionary(user => user.Login, user => user.Nome);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GrupoViewModel grupo)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<GrupoViewModel, object> httpPoster = new HttpHelper<GrupoViewModel, object>(factory, options.ApiHost, token);
                grupo.DataCriacao = DateTime.Now;
                grupo.Status = true;
                grupo.UsuarioCriacao = this.User.Claims.GetUserNameFromClaims()!;
                GrupoViewModel? created = await httpPoster.Post(options.ApiGrupo, grupo);

                if (created == null)
                {
                    ViewData["Error"] = "Falha na criação";
                    return View(grupo);
                }

                List<string> selectedUsers = this.Request.Form.Where(f => f.Key.Contains("user_")).Select(f => f.Value.ToString()).ToList();

                if (selectedUsers.Count() > 0)
                {
                    HttpHelper<string, object> httpRelPoster = new HttpHelper<string, object>(factory, options.ApiHost, token);
                    List<string?> results = new List<string?>();
                    bool success = true;

                    foreach (string user in selectedUsers)
                    {
                        string? result = await httpRelPoster.Post($"{options.ApiRelationalUsuarioGrupo}?login={user}&groupId={created!.Id}", new { });
                        success &= ProcessRelationOutput(results, result);
                    }

                    ViewData["Error"] = results.Where(res => !(res?.InvariantEquals("true")).GetValueOrDefault());
                }

                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<GrupoViewModel, GrupoViewModel> httpGetter = new HttpHelper<GrupoViewModel, GrupoViewModel>(factory, options.ApiHost, token);
                GrupoViewModel? original = await httpGetter.Get($"{options.ApiGrupo}/{id}");

                if (original == null)
                {
                    ViewData["Error"] = "Grupo não foi encontrado";
                    RedirectToAction("Index");
                }
                
                List<UsuarioViewModel> allUsers = await new HttpHelper<List<UsuarioViewModel>, object>(factory, options.ApiHost, token).Get(options.ApiUsuario) ??
                                              new List<UsuarioViewModel>();
                ViewData["Users"] = allUsers.ToDictionary(user => user.Login, user => user.Nome);

                return View(original);
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
                string? token = this.Request.Cookies.GetTokenFromCookies();
                HttpHelper<GrupoViewModel, GrupoViewModel> httpHelper = new HttpHelper<GrupoViewModel, GrupoViewModel>(factory, options.ApiHost, token);
                grupo.DataModificacao = DateTime.Now;
                grupo.UsuarioModificacao = this.User.Claims.GetUserNameFromClaims();
                GrupoViewModel? editedGrupo = await httpHelper.Put(options.ApiGrupo, grupo);

                if (editedGrupo == null)
                {
                    throw new Exception(UPDATE_FAILURE_MSG);
                }

                HttpHelper<string, object> httpRelHelper = new HttpHelper<string, object>(factory, options.ApiHost, token);
                string? resultDelAll = await httpRelHelper.Delete($"{options.ApiRelationalUsuarioGrupo}/{grupo.Id}");

                if (!(resultDelAll?.Equals("true")).GetValueOrDefault())
                {
                    throw new Exception(UPDATE_FAILURE_MSG);
                }


                List<string> selectedUsers = this.Request.Form.Where(f => f.Key.Contains("user_")).Select(f => f.Value.ToString()).ToList();

                if (selectedUsers.Count() > 0)
                {
                    HttpHelper<string, object> httpRelPoster = new HttpHelper<string, object>(factory, options.ApiHost, token);
                    List<string?> results = new List<string?>();
                    bool success = true;

                    foreach (string user in selectedUsers)
                    {
                        string? result = await httpRelPoster.Post($"{options.ApiRelationalUsuarioGrupo}?login={user}&groupId={grupo.Id}", new { });
                        success &= ProcessRelationOutput(results, result);
                    }

                    ViewData["Error"] = results.Where(res => !(res?.InvariantEquals("true")).GetValueOrDefault());
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(UPDATE_FAILURE_MSG))
                {
                    ViewData["Error"] = ex.Message;
                    return View(grupo);
                }
                else
                {
                    throw;
                }
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

                if (status)
                {
                    result = await httpHelper.Delete($"{options.ApiGrupo}/{id}");
                }
                else
                {
                    result = await httpHelper.Put($"{options.ApiGrupo}/{id}", new { });
                }

                return Json(new { result });
            }
            catch
            {
                throw;
            }
        }

        private bool ProcessRelationOutput(List<string?> list, string? result)
        {
            list.Add(result);
            bool.TryParse(result, out bool resBool);
            return resBool;
        }
    }
}
