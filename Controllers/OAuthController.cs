using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Web_Calendar_Api.Controllers
{
    public class OAuthController : Controller
    {
        public void Callback(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                this.GetTokens(code);
            }
        }

        public ActionResult GetTokens(string code)
        {
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var credentialsFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

            string apiURL = "https://oauth2.googleapis.com/token";
            
            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("code", code);
            request.AddQueryParameter("grant_type", "authorization_code");
            request.AddQueryParameter("redirect_uri", "https://localhost:7216/oauth/callback");

            var response = restClient.PostAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(tokenFile, response.Result.Content);
                return RedirectToAction("Index", "Home", new { status = "success" });
            }

            return View("Error");
        }

        public ActionResult RefreshToken()
        {
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var credentialsFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            string apiURL = "https://oauth2.googleapis.com/token";

            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("grant_type", "refresh_token");
            request.AddQueryParameter("refresh_token", tokens["refresh_token"].ToString());

            var response = restClient.PostAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject newToken = JObject.Parse(response.Result.Content);
                //la respuesata no trae el refresh token nuevamente por lo q lo debemos guardar primero
                newToken["refresh_token"] = tokens["refresh_token"].ToString(); 
                System.IO.File.WriteAllText(tokenFile, newToken.ToString());
                return RedirectToAction("Index", "Home", new { status = "success"});
            }

            return View("Error");
        }

        public ActionResult RevokeToken()
        {
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            string apiURL = "https://oauth2.googleapis.com/revoke";

            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            request.AddQueryParameter("token", tokens["access_token"].ToString());

            var response = restClient.PostAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success" });
            }

            return View("Error");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
