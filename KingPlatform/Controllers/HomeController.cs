using System.Web.Mvc;

namespace KingPlatform.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
    }
}