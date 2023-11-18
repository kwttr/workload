﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

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
            ReportVM reportVM = new ReportVM()
            {
                report = new Report(),
                teacher = _teachRepo.Find(id)
            };
            reportVM.report.TeacherId = id;
            return View(reportVM);
        }

        //POST - CREATENEWREPOPRT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewReport(ReportVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.report.Teacher = _teacherRepo.FirstOrDefault(u => u.Id == obj.report.TeacherId, includeProperties: "Degree");
                obj.report.CurrentDegree = obj.report.Teacher.Degree.Name;
                obj.report.Rate = 1;
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
    }
}
