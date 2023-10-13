using Microsoft.AspNetCore.Mvc;

namespace workload.Controllers
{
    public class ApplicationTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
