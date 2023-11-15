using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using workload_Models;
using workload_Models.ViewModels;

namespace workload.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;

        public ProfileController(UserManager<IdentityUser> userManager, RoleManager<CustomRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
                List<ProfileVM> objList = new List<ProfileVM>();
                foreach(var role in roles) 
                {
                    var userRole = _roleManager.FindByNameAsync(role).Result;
                    var regname = Regex.Replace(userRole.Name, @"\d", "");
                    objList.Add(new ProfileVM
                    {
                        Role = regname,
                        DepartmentId = userRole.DepartmentId
                    });
                }
                return View(objList);
            }
        }
    }
}
