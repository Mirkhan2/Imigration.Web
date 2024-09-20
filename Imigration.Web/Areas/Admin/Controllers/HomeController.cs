using Microsoft.AspNetCore.Mvc;

namespace Imigration.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
