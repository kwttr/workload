using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;

namespace workload.Controllers
{
    [Authorize]
    public class TeacherProfileController : Controller
    {
        private readonly IReportRepository _repRepo;
        private readonly ITeacherRepository _teachRepo;
        private readonly UserManager<IdentityUser> _userManager;


        public TeacherProfileController(
            IReportRepository reportRepository, 
            ITeacherRepository teachRepo,
            UserManager<IdentityUser> userManager)
        {
            _repRepo = reportRepository;
            _teachRepo = teachRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            TeacherProfileVM vm = new TeacherProfileVM()
            {
                teacher = _teachRepo.Find(user.Id.FirstOrDefault())
            };
            //vm.reportList = _repRepo.GetAll().Where(x => x.TeacherId == vm.teacher.Id).ToList();

            List<Report> reports = new List<Report>();
            var matchingReports = _repRepo.GetAll().Where(u => u.TeacherId== vm.teacher.Id).ToList();
            foreach (var report in matchingReports)
            {
                reports.Add(report);
            }

            vm.reportList = reports;
            return View();
        }
    }
}
