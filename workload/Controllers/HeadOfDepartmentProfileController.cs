﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility.ClaimTypes;

namespace workload.Controllers
{
    [Authorize]
    public class HeadOfDepartmentProfileController : Controller
    {
        private readonly ITeacherRepository _teachRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReportRepository _repRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProcessActivityTypeRepository _processActivityTypeRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IActivityTypeRepository _activityTypeRepo;
        private readonly ITeacherDepartmentRepository _teacherDepartmentRepo;

        public HeadOfDepartmentProfileController(
            ITeacherRepository teachRepo,
            UserManager<IdentityUser> userManager,
            IReportRepository reportRepo,
            ICategoryRepository categoryRepo,
            IProcessActivityTypeRepository processActivityTypeRepo,
            ITeacherRepository teacherRepo,
            IActivityTypeRepository activityTypeRepo,
            ITeacherDepartmentRepository teacherDepartmentRepo)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
            _repRepo = reportRepo;
            _categoryRepo = categoryRepo;
            _processActivityTypeRepo = processActivityTypeRepo;
            _teacherRepo = teacherRepo;
            _activityTypeRepo = activityTypeRepo;
            _teacherDepartmentRepo = teacherDepartmentRepo;
        }

        public IActionResult Index(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            HeadOfDepartmentProfileVM vm = new HeadOfDepartmentProfileVM()
            {
                Teacher = _teachRepo.Find(user.Id),
                ListTeacher = new List<Teacher>()
            };
            var listTeachers = _teacherDepartmentRepo.GetAll(d=>d.DepartmentId==id);
            foreach(var teacher in listTeachers)
            {
                var teachuser = _userManager.FindByIdAsync(teacher.TeacherId).Result;
                var roles = _userManager.GetRolesAsync(teachuser).Result;
                if (roles.Any(x => x.Contains("Teacher"))){
                    vm.ListTeacher.Add(_teachRepo.FirstOrDefault(x => x.Id == teacher.TeacherId,includeProperties: "Reports"));
                }
            }
            return View(vm);
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

        //GET - VIEWALLREPORTS
        public IActionResult ViewAllReports(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                TeacherProfileVM vm = new TeacherProfileVM()
                {
                    teacher = _teachRepo.Find(id)
                };
                List<Report> reports = new List<Report>();
                var matchingReports = _repRepo.GetAll().Where(u => u.TeacherId == vm.teacher.Id).ToList();
                matchingReports.Reverse();
                foreach (var report in matchingReports)
                {
                    reports.Add(report);
                }

                vm.reportList = reports;
                return View(vm);
            }
        }

        //APPROVEREPORT
        public IActionResult ApproveReport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Report obj = _repRepo.Find(id.GetValueOrDefault());
                obj.StatusId = 3;
                _repRepo.Update(obj);
                _repRepo.Save();
                return RedirectToAction("ViewAllReports", new { Id = obj.TeacherId});
            }
        }

        //DECLINEREPORT
        public IActionResult DeclineReport(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                Report obj = _repRepo.Find(id.GetValueOrDefault());
                obj.StatusId = 1;
                _repRepo.Update(obj);
                _repRepo.Save();
                return RedirectToAction("ViewAllReports", new { Id = obj.TeacherId });
            }
        }

        //GET - CREATENEWREPORT
        public IActionResult CreateNewReport(string? id)
        {
            if (id != null)
            {

                ReportVM reportVm = new ReportVM()
                {
                    report = new Report(),
                    teacher = _teachRepo.Find(id)
                };
                reportVm.report.TeacherId = id;

                reportVm.report.Rate = 1.0;
                return View(reportVm);
            }
            else return BadRequest();
        }

        //POST - CREATENEWREPOPRT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewReport(ReportVM obj)
        {
            var user = _userManager.GetUserAsync(User).Result;
            List<CustomClaimValue> deserializedClaims = new List<CustomClaimValue>();
            var claims = User.Claims.Where(c => c.Type == "UserRoleDep");
            foreach (var claim in claims)
            {
                deserializedClaims.Add(JsonConvert.DeserializeObject<CustomClaimValue>(claim.Value) ?? throw new InvalidOperationException());
            }
            var teacher = _teacherRepo.Find(user.Id);
            var deserializedClaim = deserializedClaims.FirstOrDefault(x => x.RoleAccess == "HeadOfDepartment");
            obj.report.hodName = teacher.FirstName;
            obj.report.hodSecondName = teacher.LastName;
            obj.report.hodPatronymic = teacher.Patronymic;
            if (deserializedClaim != null) obj.report.DepartmentId = Convert.ToInt32(deserializedClaim.DepartmentId);
            else return BadRequest();
            ModelState.Remove("report.hodSecondName");
            ModelState.Remove("report.hodName");
            ModelState.Remove("report.hodPatronymic");
            if (ModelState.IsValid)
            {
                obj.report.Teacher = _teacherRepo.FirstOrDefault(u => u.Id == obj.report.TeacherId, includeProperties: "Degree,Position");
                if ((obj.report.Teacher.Degree != null) && (obj.report.Teacher.Position != null))
                {
                    obj.report.CurrentDegree = obj.report.Teacher.Degree.Name;
                    obj.report.CurrentPosition = obj.report.Teacher.Position.Name;
                }
                else return BadRequest();
                obj.report.StatusId = 1;
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
                return RedirectToAction("ViewAllReports",new {Id = obj.report.TeacherId});
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
                if (obj.Department != null && obj.Teacher != null)
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{obj.Title + "_" + obj.Department.Name + "_" + obj.Teacher.LastName + obj.Teacher.FirstName + obj.Teacher.Patronymic}.docx");
                else return BadRequest();
            }
        }
    }
}
