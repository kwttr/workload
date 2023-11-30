﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportController(IReportRepository repRepo,
                                ICategoryRepository categoryRepo,
                                ITeacherRepository teacherRepo,
                                IActivityTypeRepository activityTypeRepo,
                                IProcessActivityTypeRepository processActivityTypeRepository,
                                IHttpContextAccessor contextAccessor,
                                UserManager<IdentityUser> userManager)
        {
            _repRepo = repRepo;
            _categoryRepo = categoryRepo;
            _teacherRepo = teacherRepo;
            _activityTypeRepo = activityTypeRepo;
            _processActivityTypeRepo = processActivityTypeRepository;
            _contextAccessor = contextAccessor;
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
            var user = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;
            List<CustomClaim> deserializedClaims = new List<CustomClaim>();
            var claims = User.Claims.Where(c => c.Type == "UserRoleDep");
            foreach (var claim in claims)
            {
                deserializedClaims.Add(JsonConvert.DeserializeObject<CustomClaim>(claim.Value));
            }
            var deserializedClaim = deserializedClaims.FirstOrDefault(x => x.RoleAccess == "HeadOfDepartment");
            if (ModelState.IsValid)
            {
                obj.report.Teacher = _teacherRepo.FirstOrDefault(u => u.Id == obj.report.TeacherId, includeProperties: "Degree");
                obj.report.CurrentDegree = obj.report.Teacher.Degree.Name;
                obj.report.StatusId = 1;
                var User = _teacherRepo.FirstOrDefault(x => x.Id == user.Id);
                obj.report.hodName = User.FirstName;
                obj.report.hodSecondName = User.LastName;
                obj.report.hodPatronymic = User.Patronymic;
                obj.report.DepartmentId = Convert.ToInt32(deserializedClaim.DepartmentId);
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
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Document.docx");
            }
        }
    }
}