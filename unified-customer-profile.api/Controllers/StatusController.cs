using Microsoft.AspNetCore.Mvc;

namespace unified_customer_profile.api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
