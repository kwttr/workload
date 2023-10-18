using Microsoft.AspNetCore.Mvc;

namespace workload.Controllers
{
    public class TeacherProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
