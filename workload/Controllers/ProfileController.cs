using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using workload_Models;

namespace workload.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<Teacher> _userManager;

        public ProfileController(UserManager<Teacher> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var roles = _userManager.GetRolesAsync(user).Result;
            }
            return View();
        }
    }
}
