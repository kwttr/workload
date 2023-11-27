using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility.ClaimTypes;

namespace workload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeacherDepartmentRepository _teacherDepartmentRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IReportRepository _reportRepository;

        public HomeController(ILogger<HomeController> logger, ITeacherDepartmentRepository teacherDepartmentRepository, IDepartmentRepository departmentRepository, IReportRepository reportRepository)
        {
            _logger = logger;
            _teacherDepartmentRepository = teacherDepartmentRepository;
            _departmentRepository = departmentRepository;
            _reportRepository = reportRepository;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claims = User.Claims.Where(c => c.Type == "UserRoleDep");
                List<CustomClaim> deserializedClaims = new List<CustomClaim>();
                foreach (var claim in claims)
                {
                    deserializedClaims.Add(JsonConvert.DeserializeObject<CustomClaim>(claim.Value));
                }

                if (deserializedClaims.Any(x=>x.RoleAccess=="HeadOfDepartment"))
                {
                    HomeViewModel homeVM = new HomeViewModel();
                    List<DepartmentHodWindow> depWindows = new List<DepartmentHodWindow>();
                    foreach (var claim in deserializedClaims.Where(x => x.RoleAccess == "HeadOfDepartment"))
                    {
                        IEnumerable<TeacherDepartment> teachdeps = _teacherDepartmentRepository.GetAll(x => x.DepartmentId == Convert.ToInt32(claim.DepartmentId));
                        DepartmentHodWindow depWindow = new DepartmentHodWindow();
                        depWindow.Department = _departmentRepository.Find(Convert.ToInt32(claim.DepartmentId));
                        depWindow.WorkersCount = teachdeps.Count();
                        depWindow.ReportsToApproveCount = _reportRepository.GetAll(x => x.DepartmentId == Convert.ToInt32(claim.DepartmentId) && (x.StatusId == 2)).Count();
                        depWindows.Add(depWindow);
                    }
                    homeVM.DepartmentsHod = depWindows;
                    return View(homeVM);
                }

                if (deserializedClaims.Any(x => x.RoleAccess == "Teacher"))
                {
                    HomeViewModel homeVM = new HomeViewModel();
                    List<DepartmentTeacherWindow> depWindows = new();
                    foreach(var claim in deserializedClaims.Where(x=>x.RoleAccess == "Teacher"))
                    {
                        DepartmentTeacherWindow depWindow = new();
                        depWindow.Department = _departmentRepository.Find(Convert.ToInt32(claim.DepartmentId));
                        depWindow.ReportsAssignedCount = _reportRepository.GetAll(x => x.DepartmentId == Convert.ToInt32(claim.DepartmentId) && (x.StatusId == 1)).Count();
                        depWindows.Add(depWindow);
                    }
                    homeVM.DepartmentsTeacher = depWindows;
                    return View(homeVM);
                }
                return View();
            }
            else return Redirect("Identity/Account/Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}