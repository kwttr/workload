using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
            //vm.reportList = _repRepo.GetAll().Where(x => x.TeacherId == vm.teacher.Id).ToList();

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
            ReportDetailsVM reportDetailsVM = new ReportDetailsVM()
            {
                Report = _repRepo.Find(id.GetValueOrDefault()),
                CategoryList = _categoryRepo.GetAll().ToList(),
            };

            List<ProcessActivityType> processActivityList = new List<ProcessActivityType>();
            var matchingProcessActivities = _processActivityTypeRepo.GetAll().Where(u => u.ReportId == reportDetailsVM.Report.Id).ToList();
            foreach (var processActivity in matchingProcessActivities)
            {
                processActivityList.Add(processActivity);
            }
            reportDetailsVM.ProcessActivityTypes = processActivityList;

            if (reportDetailsVM == null)
            {
                return NotFound();
            }
            return View(reportDetailsVM);
        }

        //POST - UPDATEACTIVITIES
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateActivities(ReportDetailsVM reportDetailsVM)
        {
            if(ModelState.IsValid)
            {
                for(int i = 0; i < reportDetailsVM.ProcessActivityTypes.Count(); i++)
                {
                    _processActivityTypeRepo.Update(reportDetailsVM.ProcessActivityTypes[i]);
                }
                _processActivityTypeRepo.Save();
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
            ReportDetailsVM reportDetailsVM = new ReportDetailsVM()
            {
                Report = _repRepo.Find(id.GetValueOrDefault()),
                CategoryList = _categoryRepo.GetAll().ToList(),
            };
            if (reportDetailsVM.Report == null)
            {
                return NotFound();
            }
            List<ProcessActivityType> processActivityList = new List<ProcessActivityType>();
            var matchingProcessActivities = _processActivityTypeRepo.GetAll().Where(u => u.ReportId == reportDetailsVM.Report.Id).ToList();
            foreach (var processActivity in matchingProcessActivities)
            {
                processActivityList.Add(processActivity);
            }
            reportDetailsVM.ProcessActivityTypes = processActivityList;

            if (reportDetailsVM == null)
            {
                return NotFound();
            }
            return View(reportDetailsVM);
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
                Report obj = _repRepo.Find(id.GetValueOrDefault());
                MemoryStream stream = _repRepo.Export(obj);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Document.docx");
            }
        }
    }
}
