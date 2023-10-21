﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.HeadOfDepartmentRole)]
    public class HeadOfDepartmentProfileController : Controller
    {
        private readonly ITeacherRepository _teachRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReportRepository _repRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProcessActivityTypeRepository _processActivityTypeRepo;

        public HeadOfDepartmentProfileController(
            ITeacherRepository teachRepo,
            UserManager<IdentityUser> userManager,
            IReportRepository reportRepo,
            ICategoryRepository categoryRepo,
            IProcessActivityTypeRepository processActivityTypeRepo)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
            _repRepo = reportRepo;
            _categoryRepo = categoryRepo;
            _processActivityTypeRepo = processActivityTypeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            HeadOfDepartmentProfileVM vm = new HeadOfDepartmentProfileVM()
            {
                Teacher = _teachRepo.Find(user.Id)
            };
            vm.ListTeacher = _teachRepo.GetAll(includeProperties:"Reports").Where(x => x.DepartmentId == vm.Teacher.DepartmentId);
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
                foreach (var report in matchingReports)
                {
                    reports.Add(report);
                }

                vm.reportList = reports;
                return View(vm);
            }
        }
    }
}