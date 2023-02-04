using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static KnowledgeBaseForum.AdminWebApp.Utils.StubApi;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    public class TopicoController : Controller
    {
        private readonly string apiHost;
        private readonly string apiTopicos;
        private readonly string apiTags;
        private readonly IHttpClientFactory factory;
        private readonly bool stubMode;

        public TopicoController(IConfiguration config, IHttpClientFactory factory)
        {
            apiHost = config[Constants.API_HOST];
            apiTopicos = config[Constants.API_TOPICO];
            apiTags = config[Constants.API_TAGS];
            this.factory = factory;
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
            IEnumerable<Guid> selectedTags = ((string)this.Request.Form["tags"]).Split(",").Select(s => new Guid(s));

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
                    HttpHelper<TopicoViewModel, object> httpHelper = new HttpHelper<TopicoViewModel, object>(factory, apiHost);
                    original = await httpHelper.Get($"{apiTopicos}/{topico.Id}");

                    if (original == null)
                    {
                        return NotFound();
                    }

                    original.TagLinks = selectedTags?.Select(t => new TopicoTagLink() { TagId = t, TopicoId = original.Id });
                    TopicoViewModel? edited = await httpHelper.Put($"{apiTopicos}/{topico.Id}", topico);
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

            if (stubMode)
            {
                TopicoViewModel original = SimulateGetTopico(id)!;
                original.Status = !status;
                SimulateEditTopico(original);
            }
            else
            {
                try
                {
                    HttpHelper<bool, object> httpHelper = new HttpHelper<bool, object>(factory, apiHost);
                    result = status ? await httpHelper.Delete($"{apiTopicos}/{id}") : await httpHelper.Put($"{apiTopicos}/{id}", new { });
                }
                catch
                {
                    throw;
                }
            }

            return Json(new { result });
        }
    }
}
