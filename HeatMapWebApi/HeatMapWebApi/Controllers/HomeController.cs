using System.Net.Http;
using System.Web.Mvc;
using HeatMapWebApi.Repositories;

namespace HeatMapWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //HttpClient httpClient = new HttpClient();

            //var response = httpClient.GetAsync("http://localhost:57750/HeatMap");

            //var model = response.Result;

            return View();
        }
    }
}
