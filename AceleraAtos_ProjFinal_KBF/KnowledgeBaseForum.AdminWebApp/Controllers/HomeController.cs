using KnowledgeBaseForum.AdminWebApp.Models.API;
using KnowledgeBaseForum.AdminWebApp.Models.Config;
using KnowledgeBaseForum.AdminWebApp.Models.ViewModel;
using KnowledgeBaseForum.AdminWebApp.Utils;
using KnowledgeBaseForum.Commons.JWT;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static KnowledgeBaseForum.AdminWebApp.Utils.Constants;

namespace KnowledgeBaseForum.AdminWebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApiOptions options;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory factory;
        private readonly string cypherKey;
        private readonly ITokenService tokenService;

        public HomeController(IConfiguration config, IHttpClientFactory factory, ILogger<HomeController> logger, IOptions<ApiOptions> options, ITokenService tokenService)
        {
            this.options = options.Value;
            cypherKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(config[CYPHER_SECRET]));
            this.factory = factory;
            _logger = logger;
            this.tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("Usuário ou Senha inválidos");
                }

                string encodedPwd = $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}.{cypherKey}";
                LoginResponse? response = await new HttpHelper<LoginResponse, Login>(factory, options.ApiHost).Post(options.ApiAuthLogin, new Login(login, encodedPwd));

                if (response == null)
                {
                    throw new ArgumentNullException("Falha no login");
                }

                this.Response.Cookies.Append(JWT_COOKIE_KEY, response!.Token!, new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(480),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });
                IEnumerable<Claim> userClaims = tokenService.ExtractClaimsFromToken(response!.Token!);
                ClaimsIdentity identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(480)
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            this.Response.Cookies.Delete(JWT_COOKIE_KEY);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}