using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using workload_Utility;
using System.Data;
using workload_Data;
using workload_Models;
using workload_Models.ViewModels;
using workload_DataAccess.Repository.IRepository;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ReportController : Controller
    {
        private readonly IReportRepository _repRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IActivityTypeRepository _activityTypeRepo;
        private readonly IProcessActivityTypeRepository _processActivityTypeRepo;

        public ReportController(IReportRepository repRepo,
                                ICategoryRepository categoryRepo,
                                ITeacherRepository teacherRepo,
                                IActivityTypeRepository activityTypeRepo,
                                IProcessActivityTypeRepository processActivityTypeRepository)
        {
            _repRepo = repRepo;
            _categoryRepo = categoryRepo;
            _teacherRepo = teacherRepo;
            _activityTypeRepo = activityTypeRepo;
            _processActivityTypeRepo = processActivityTypeRepository;
        }
        [BindProperty]
        public List<ProcessActivityType> processActivityTypes { get; set; }

        public IActionResult Index()
        {
            IEnumerable<Report> objList = _repRepo.GetAll(includeProperties:"Teacher");
            return View(objList);
        }
        
        //GET - DETAILS
        public IActionResult Details(int? id)
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
            var matchingProcessActivities = _processActivityTypeRepo.GetAll().Where(u=>u.ReportId == reportDetailsVM.Report.Id).ToList();
            foreach(var processActivity in matchingProcessActivities)
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

        //GET - CREATE
        public IActionResult Create()
        {
            ReportVM reportVM = new ReportVM()
            {
                report = new Report(),
                TeacherSelectList = _repRepo.GetAllDropdownList(WC.TeacherName)
            };
            return View(reportVM);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.report.Teacher = _teacherRepo.FirstOrDefault(u => u.Id == obj.report.TeacherId,includeProperties:"Degree");
                obj.report.CurrentDegree = obj.report.Teacher.Degree.Name;
                obj.report.Rate = 1;
                obj.report.StatusId = 1;
                List<ProcessActivityType> list = new List<ProcessActivityType>();
                foreach(var activity in _activityTypeRepo.GetAll())
                {
                    ProcessActivityType procAct = new ProcessActivityType()
                    {
                        Name = activity.Name,
                        NormHours = activity.NormHours,
                        HoursPlan = 0,
                        HoursFact = 0,
                        UnitFact = 0,
                        UnitPlan = 0,
                        Report = obj.report,
                        CategoryId = activity.CategoryId
                    };
                    list.Add(procAct);
                }
                obj.report.ProcessActivities = list;
                _repRepo.Add(obj.report);
                _repRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}