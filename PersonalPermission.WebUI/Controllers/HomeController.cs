using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalPermission.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        [Route("anasayfa")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
