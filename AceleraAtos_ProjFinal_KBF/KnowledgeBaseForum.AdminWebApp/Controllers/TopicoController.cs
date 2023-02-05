using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static KnowledgeBaseForum.AdminWebApp.Utils.StubApi;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class TopicoController : Controller
    {
        private readonly string apiHost;
        private readonly string apiRelRopicoTags;
        private readonly string apiTags;
        private readonly string apiTopicos;
        private readonly IHttpClientFactory factory;
        private readonly bool stubMode;

        public TopicoController(IConfiguration config, IHttpClientFactory factory)
        {
            this.factory = factory;
            apiHost = config[Constants.API_HOST];
            apiRelRopicoTags = config[Constants.API_REL_TOPICO_TAG];
            apiTopicos = config[Constants.API_TOPICO];
            apiTags = config[Constants.API_TAGS];
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
                    HttpHelper<IEnumerable<TopicoViewModel>, object> httpGetter = new HttpHelper<IEnumerable<TopicoViewModel>, object>(factory, apiHost);
                    queried = await httpGetter.Get(apiTopicos) ?? new List<TopicoViewModel>();
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
                    HttpHelper<TopicoViewModel, object> httpGetter = new HttpHelper<TopicoViewModel, object>(factory, apiHost);
                    original = await httpGetter.Get($"{apiTopicos}/{id}");
                    IEnumerable<TagViewModel>? tags = await new HttpHelper<IEnumerable<TagViewModel>, object>(factory, apiHost).Get(apiTags);
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
                    original = await new HttpHelper<TopicoViewModel, object>(factory, apiHost).Get($"{apiTopicos}/{topico.Id}");

                    if (original == null)
                    {
                        return NotFound();
                    }

                    bool success = true;
                    List<string?> results = new List<string?>();
                    HttpHelper<string, object> httpHelper = new HttpHelper<string, object>(factory, apiHost);

                    foreach (Guid tagId in selectedTags.Where(tag => !(original?.TagLinks?.Select(tl => tl.TagId).Contains(tag)).GetValueOrDefault()))
                    {
                        string? result = await httpHelper.Post($"{apiRelRopicoTags}?tagId={tagId}&topicId={original?.Id}", new { tagId, topicId = original?.Id });
                        success &= ProcessRelationOutput(results, result);
                    }

                    foreach (Guid tagId in (original?.TagLinks?.Where(tl => !selectedTags.Contains(tl.TagId)).Select(tl => tl.TagId) ?? new Guid[] { }))
                    {
                        string? result = await httpHelper.Delete($"{apiRelRopicoTags}?tagId={tagId}&topicId={original?.Id}");
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
                    if (status)
                    {
                        HttpHelper<bool, object> httpHelper = new HttpHelper<bool, object>(factory, apiHost);
                        result = await httpHelper.Delete($"{apiTopicos}/{id}");
                    }
                    else
                    {
                        HttpHelper<TopicoViewModel, object> httpHelper = new HttpHelper<TopicoViewModel, object>(factory, apiHost);
                        original = await httpHelper.Get($"{apiTopicos}/{id}");

                        if (original != null)
                        {
                            original.Status = true;
                            result = await httpHelper.Put($"{apiTopicos}/{id}", original) != null;
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
