using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using Web_Calendar_Api.Models;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Calendar.v3;
//using Google.Apis.Calendar.v3.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;

namespace Web_Calendar_Api.Controllers
{
    public class CalendarEventController : Controller
    {
        public ActionResult CreateEvent(Web_Calendar_Api.Models.Event calendarEvent)
        {
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            string apiURL = "https://www.googleapis.com/calendar/v3/calendars/primary/events";

            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:nn:ss.fff");
            calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.End.DateTime).ToString("yyyy-MM-dd'T'HH:nn:ss.fff");

            var model = JsonConvert.SerializeObject(calendarEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            request.AddQueryParameter("key", "AIzaSyAV_tlXDEcrjqd3xm1xCwMen4oQgmrcfKI");
            request.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", model, ParameterType.RequestBody);

            var response = restClient.PostAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success" });
            }
            return View("Error");
        }

        public ActionResult AllEvents()
        {
            
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            string apiURL = "https://www.googleapis.com/calendar/v3/calendars/primary/events";

            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            request.AddQueryParameter("ApiKey", "AIzaSyAV_tlXDEcrjqd3xm1xCwMen4oQgmrcfKI");
            request.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            var response = restClient.GetAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvents = JObject.Parse(response.Result.Content);
                var allEvents = calendarEvents["items"].ToObject<IEnumerable<Event>>();
                return View(AllEvents);
            }

            return View("Error");
        }

        public ActionResult Event(string identifier)
        {
            var tokenFile = "D:\\Documentos\\Proyectos\\VisualStudio2022\\Web Calendar Api\\Files\\token.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            string apiURL = "https://www.googleapis.com/calendar/v3/calendars/primary/events";

            RestClient restClient = new RestClient(apiURL);
            RestRequest request = new RestRequest();

            request.AddQueryParameter("key", "AIzaSyAV_tlXDEcrjqd3xm1xCwMen4oQgmrcfKI");
            request.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");

            var response = restClient.GetAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvent = JObject.Parse(response.Result.Content);
                return View(calendarEvent.ToObject<Web_Calendar_Api.Models.Event>());
            }

            return View("Error");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
