using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using workload_Utility;
using System.Data;
using workload_Data;
using workload_Models;
using workload_Models.ViewModels;
using workload_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using workload_Utility.ClaimTypes;
using DocumentFormat.OpenXml.Office2010.Excel;

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
        private readonly UserManager<IdentityUser> _userManager;

        public ReportController(IReportRepository repRepo,
                                ICategoryRepository categoryRepo,
                                ITeacherRepository teacherRepo,
                                IActivityTypeRepository activityTypeRepo,
                                IProcessActivityTypeRepository processActivityTypeRepository,
                                UserManager<IdentityUser> userManager)
        {
            _repRepo = repRepo;
            _categoryRepo = categoryRepo;
            _teacherRepo = teacherRepo;
            _activityTypeRepo = activityTypeRepo;
            _processActivityTypeRepo = processActivityTypeRepository;
            _userManager = userManager;
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
            var user = _userManager.GetUserAsync(User).Result;
            List<CustomClaim> deserializedClaims = new List<CustomClaim>();
            var claims = User.Claims.Where(c => c.Type == "UserRoleDep");
            foreach (var claim in claims)
            {
                deserializedClaims.Add(JsonConvert.DeserializeObject<CustomClaim>(claim.Value));
            }
            var teacher = _teacherRepo.Find(user.Id);
            var deserializedClaim = deserializedClaims.FirstOrDefault(x => x.RoleAccess == "HeadOfDepartment");
            var name = teacher.FirstName;
            var secondname = teacher.LastName;
            var patronymic = teacher.Patronymic;
            int depId = 0;
            if (deserializedClaim != null) depId = Convert.ToInt32(deserializedClaim.DepartmentId);
            else return BadRequest();
            ReportVM reportVM = new ReportVM()
            {
                report = new Report(teacher.Id, name, secondname, patronymic, depId),
                TeacherSelectList = _repRepo.GetAllDropdownList(WC.TeacherName)
            };
            reportVM.report.Title = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            reportVM.report.Rate = 1.0;
            return View(reportVM);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportVM obj)
        {
            if (ModelState.IsValid)
            {
                List<ProcessActivityType> list = new List<ProcessActivityType>();
                foreach (var activity in _activityTypeRepo.GetAll())
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

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Report report = _repRepo.FirstOrDefault(r => r.Id == id);
                return View(report);
            }
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                _repRepo.Update(report);
                _repRepo.Save();
            }
            return RedirectToAction("Index");
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Report report = _repRepo.FirstOrDefault(r => r.Id == id);
                return View(report);
            }
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Report report)
        {
            if (ModelState.IsValid)
            {
                _repRepo.Remove(report);
                _repRepo.Save();
            }
            return RedirectToAction("Index");
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
                Report obj = _repRepo.FirstOrDefault(r => r.Id == id, includeProperties: "ProcessActivities,Teacher,Department");
                MemoryStream stream = _repRepo.Export(obj);
                if (obj != null && obj.Department != null && obj.Teacher != null)
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{obj.Title + "_" + obj.Department.Name + "_" + obj.Teacher.LastName + obj.Teacher.FirstName + obj.Teacher.Patronymic}.docx");
                else return BadRequest();
            }
        }
    }
}