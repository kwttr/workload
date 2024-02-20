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
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProcessActivityTypeRepository _processActivityTypeRepo;
        private readonly UserManager<IdentityUser> _userManager;


        public TeacherProfileController(
            IReportRepository reportRepository, 
            ITeacherRepository teachRepo,
            UserManager<IdentityUser> userManager,
            ICategoryRepository categoryRepo,
            IProcessActivityTypeRepository processActivityTypeRepo)
        {
            _repRepo = reportRepository;
            _teachRepo = teachRepo;
            _userManager = userManager;
            _categoryRepo = categoryRepo;
            _processActivityTypeRepo = processActivityTypeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            TeacherProfileVM vm = new TeacherProfileVM()
            {
                teacher = _teachRepo.Find(user.Id)
            };

            List<Report> reports = new List<Report>();
            var matchingReports = _repRepo.GetAll().Where(u => u.TeacherId== vm.teacher.Id).ToList();
            foreach (var report in matchingReports)
            {
                reports.Add(report);
            }

            vm.reportList = reports;
            return View(vm);
        }

        //GET - UPDATEACTIVITIES
        public IActionResult UpdateActivities(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            ReportDetailsVM reportDetailsVm = new ReportDetailsVM()
            {
                Report = _repRepo.Find(id.GetValueOrDefault()),
                CategoryList = _categoryRepo.GetAll().ToList(),
            };

            List<ProcessActivityType> processActivityList = new List<ProcessActivityType>();
            var matchingProcessActivities = _processActivityTypeRepo.GetAll().Where(u => u.ReportId == reportDetailsVm.Report.Id).ToList();
            foreach (var processActivity in matchingProcessActivities)
            {
                processActivityList.Add(processActivity);
            }
            reportDetailsVm.ProcessActivityTypes = processActivityList;

            return View(reportDetailsVm);
        }

        //POST - UPDATEACTIVITIES
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateActivities(ReportDetailsVM reportDetailsVm)
        {
            ModelState.Remove("report.hodSecondName");
            ModelState.Remove("report.hodName");
            ModelState.Remove("report.hodPatronymic");
            if (ModelState.IsValid)
            {
                
                for(int i = 0; i < reportDetailsVm.ProcessActivityTypes.Count(); i++)
                {
                    _processActivityTypeRepo.Update(reportDetailsVm.ProcessActivityTypes[i]);
                }
                reportDetailsVm.Report.ProcessActivities = reportDetailsVm.ProcessActivityTypes;
                _processActivityTypeRepo.Save();
                _repRepo.Update(reportDetailsVm.Report);
                _repRepo.Save();
            }
            return RedirectToAction("Index");
        }

        //SENDREPORT
        public IActionResult SendReport(int? id)
        {
            if(id != null)
            {
                Report obj = _repRepo.Find(id.GetValueOrDefault());
                obj.StatusId= 2;
                _repRepo.Save();
            }
            return RedirectToAction("Index");
        }

        //GET - VIEWREPORT
        public IActionResult ViewReport(int? id)
        {
            if (id == 0 || id == null || id == -1)
            {
                return NotFound();
            }
            ReportDetailsVM reportDetailsVm = new ReportDetailsVM()
            {
                Report = _repRepo.Find(id.GetValueOrDefault()),
                CategoryList = _categoryRepo.GetAll().ToList(),
            };
            List<ProcessActivityType> processActivityList = new List<ProcessActivityType>();
            var matchingProcessActivities = _processActivityTypeRepo.GetAll().Where(u => u.ReportId == reportDetailsVm.Report.Id).ToList();
            foreach (var processActivity in matchingProcessActivities)
            {
                processActivityList.Add(processActivity);
            }
            reportDetailsVm.ProcessActivityTypes = processActivityList;

            return View(reportDetailsVm);
        }

        //EXPORTREPORT
        public IActionResult ExportReport(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                Report obj = _repRepo.FirstOrDefault(r=>r.Id==id, includeProperties: "ProcessActivities,Teacher,Department");
                MemoryStream stream = _repRepo.Export(obj);
                if (obj.Department != null && obj.Teacher != null)
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{obj.Title + "_" + obj.Department.Name + "_" + obj.Teacher.LastName + obj.Teacher.FirstName + obj.Teacher.Patronymic}.docx");
                else return BadRequest();
            }
        }
    }
}
