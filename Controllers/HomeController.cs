//Seccion Google
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Web_Calendar_Api.Models;



namespace Web_Calendar_Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Esta es creada por el ejemplo 
        public ActionResult OauthRedirect()
        {
            var credentialsFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\credentials.json";

            JObject credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            var client_id = credentials["client_id"];
            var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                "access_type=offline&" + 
                "include_granted_scopes=true&" +
                "response_type=code&" +
                "state=hellothere&" +
                "redirect_uri=https://localhost:7216/oauth/callback&" +
                "client_id=" + client_id;

        
            return Redirect(redirectUrl);
        }

    }
}