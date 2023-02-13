using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KnowledgeBaseForum.AdminWebApp.Models;
using KnowledgeBaseForum.AdminWebApp.Models.API;
using KnowledgeBaseForum.AdminWebApp.Utils;
using static KnowledgeBaseForum.AdminWebApp.Utils.Constants;

namespace KnowledgeBaseForum.AdminWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory factory;
    private readonly string apiHost;
    private readonly string apiAuthLogin;
    private readonly string apiAuthSignUp;

    public HomeController(IConfiguration config, IHttpClientFactory factory, ILogger<HomeController> logger)
    {
        apiHost = config[API_HOST];
        apiAuthLogin = config[API_AUTH_LOGIN];
        apiAuthSignUp = config[API_AUTH_SIGNUP];
        this.factory = factory;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string login, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Usuário ou Senha inválidos");
            }

            LoginResponse? response = await new HttpHelper<LoginResponse, Login>(factory, apiHost).Post(apiAuthLogin, new Login(login, password));

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return View();
        }
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(string login, string password, string name, string email)
    {
        try
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                ViewData["Error"] = "Usuário ou Senha inválidos";
                return View();
            }

            UsuarioViewModel user = new UsuarioViewModel()
            {
                Email = email,
                Login = login,
                Nome = name,
                Senha = password,
                Status = true,
                UsuarioCriacao = login
            };
            LoginResponse? response = await new HttpHelper<LoginResponse, UsuarioViewModel>(factory, apiHost).Post(apiAuthSignUp, user);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
