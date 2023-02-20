using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Data;
using static KnowledgeBaseForum.AdminWebApp.Utils.StubApi;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class TopicoController : Controller
    {
        private readonly ApiOptions options;
        private readonly IHttpClientFactory factory;
        private readonly bool stubMode;

        public TopicoController(IConfiguration config, IHttpClientFactory factory, IOptions<ApiOptions> options)
        {
            this.factory = factory;
            this.options = options.Value;
            stubMode = bool.Parse(config[Constants.STUB_MODE]);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(string filter)
        {
            IEnumerable<TopicoViewModel> queried;

            if (stubMode)
            {
                queried = SimulateListTopico();
            }
            else
            {
                try
                {
                    string? token = this.Request.Cookies.GetTokenFromCookies();
                    HttpHelper<IEnumerable<TopicoViewModel>, object> httpGetter = new HttpHelper<IEnumerable<TopicoViewModel>, object>(factory, options.ApiHost, token);
                    queried = await httpGetter.Get(options.ApiTopico) ?? new List<TopicoViewModel>();
                }
                catch
                {
                    throw;
                }
            }

            return PartialView("_ListTopics", queried.Where(t => string.IsNullOrEmpty(filter) ||
                                                                 t.Titulo.InvariantContains(filter) ||
                                                                 (t.TagLinks?.Any(tl => (tl.Tag?.Descricao?.InvariantContains(filter)).GetValueOrDefault())).GetValueOrDefault()));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            TopicoViewModel? original;
            SelectList selectListForTags;

            if (stubMode)
            {
                original = SimulateGetTopico(id)!;
                selectListForTags = new SelectList(SimulateListTags(), "Id", "Descricao");
            }
            else
            {
                try
                {
                    string? token = this.Request.Cookies.GetTokenFromCookies();
                    HttpHelper<TopicoViewModel, object> httpGetter = new HttpHelper<TopicoViewModel, object>(factory, options.ApiHost, token);
                    original = await httpGetter.Get($"{options.ApiTopico}/{id}");
                    IEnumerable<TagViewModel>? tags = await new HttpHelper<IEnumerable<TagViewModel>, object>(factory, options.ApiHost, token).Get(options.ApiTags);
                    selectListForTags = new SelectList(tags, "Id", "Descricao");
                }
                catch
                {
                    throw;
                }
            }

            ViewData["TagSelect"] = selectListForTags;
            return View(original);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TopicoViewModel topico)
        {
            TopicoViewModel? original;
            IEnumerable<Guid> selectedTags = ((string)this.Request.Form["tags"])?.Split(",")?.Select(s => new Guid(s)) ?? new Guid[] { };

            if (stubMode)
            {
                original = SimulateGetTopico(topico.Id)!;
                IEnumerable<TagViewModel> tagData = SimulateListTags().Where(t => (selectedTags?.Contains(t.Id)).GetValueOrDefault()).ToList();
                original.TagLinks = selectedTags?.Select(t => new TopicoTagLink() { TagId = t, TopicoId = original.Id, Tag = tagData.First(td => td.Id == t) });
                SimulateEditTopico(topico);
            }
            else
            {
                try
                {
                    string? token = this.Request.Cookies.GetTokenFromCookies();
                    original = await new HttpHelper<TopicoViewModel, object>(factory, options.ApiHost, token).Get($"{options.ApiTopico}/{topico.Id}");

                    if (original == null)
                    {
                        return NotFound();
                    }

                    bool success = true;
                    List<string?> results = new List<string?>();
                    HttpHelper<string, object> httpHelper = new HttpHelper<string, object>(factory, options.ApiHost, token);

                    foreach (Guid tagId in selectedTags.Where(tag => !(original?.TagLinks?.Select(tl => tl.TagId).Contains(tag)).GetValueOrDefault()))
                    {
                        string? result = await httpHelper.Post($"{options.ApiRelationalTopicoTag}?tagId={tagId}&topicId={original?.Id}", new { tagId, topicId = original?.Id });
                        success &= ProcessRelationOutput(results, result);
                    }

                    foreach (Guid tagId in (original?.TagLinks?.Where(tl => !selectedTags.Contains(tl.TagId)).Select(tl => tl.TagId) ?? new Guid[] { }))
                    {
                        string? result = await httpHelper.Delete($"{options.ApiRelationalTopicoTag}?tagId={tagId}&topicId={original?.Id}");
                        success &= ProcessRelationOutput(results, result);
                    }

                    ViewData["Errors"] = results.Where(res => !(res?.InvariantEquals("true")).GetValueOrDefault());
                }
                catch
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ToggleActivation(Guid id, bool status)
        {
            bool result = true;
            TopicoViewModel? original = null;

            if (stubMode)
            {
                original = SimulateGetTopico(id)!;
                original.Status = !status;
                SimulateEditTopico(original);
            }
            else
            {
                try
                {
                    string? token = this.Request.Cookies.GetTokenFromCookies();
                    
                    if (status)
                    {
                        HttpHelper<bool, object> httpHelper = new HttpHelper<bool, object>(factory, options.ApiHost, token);
                        result = await httpHelper.Delete($"{options.ApiTopico}/{id}");
                    }
                    else
                    {
                        HttpHelper<TopicoViewModel, object> httpHelper = new HttpHelper<TopicoViewModel, object>(factory, options.ApiHost, token);
                        original = await httpHelper.Get($"{options.ApiTopico}/{id}");

                        if (original != null)
                        {
                            original.Status = true;
                            result = await httpHelper.Put($"{options.ApiTopico}/{id}", original) != null;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            return Json(new { result });
        }

        private bool ProcessRelationOutput(List<string?> list, string? result)
        {
            list.Add(result);
            bool.TryParse(result, out bool resBool);
            return resBool;
        }
    }
}
