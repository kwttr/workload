using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using workload_Utility;
using System.Data;
using workload_Data;
using workload_Models;
using workload_Models.ViewModels;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Report> objList = _db.Reports;
            foreach (var obj in objList)
            {
                obj.Teacher = _db.Teachers.FirstOrDefault(u => u.Id == obj.TeacherId);
            }
            return View(objList);
        }
        
        //GET - DETAILS
        public IActionResult Details(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            //Заполнение листа ProcessActivityTypes
            ReportDetailsVM reportDetailsVM = new ReportDetailsVM()
            {
                Report = _db.Reports.FirstOrDefault(u => u.Id == id)
            };
            var matchingProvessActivities = _db.ProcessActivityType
                .Where(pa => pa.ReportId == reportDetailsVM.Report.Id).ToList();
            List<ProcessActivityType> processActivityList = new List<ProcessActivityType>();

            foreach(var processActivity in matchingProvessActivities)
            {
                processActivityList.Add(processActivity);
            }
            reportDetailsVM.ProcessActivityTypes = processActivityList;

            //Заполнение листа CategoryList
            reportDetailsVM.CategoryList = _db.Categories.ToList();

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
                TeacherSelectList = _db.Teachers.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
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
                obj.report.Teacher = _db.Teachers.FirstOrDefault(u => u.Id == obj.report.TeacherId);
                obj.report.Teacher.Degree = _db.Degree.FirstOrDefault(u => u.Id == obj.report.Teacher.DegreeId);
                obj.report.CurrentDegree = obj.report.Teacher.Degree.Name;
                obj.report.Rate = 1;
                List<ProcessActivityType> list = new List<ProcessActivityType>();
                foreach(var activity in _db.Activities)
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
                _db.Reports.Add(obj.report);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}