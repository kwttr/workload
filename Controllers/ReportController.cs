using Microsoft.AspNetCore.Mvc;

namespace workload.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}