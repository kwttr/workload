using Microsoft.AspNetCore.Mvc;
using workload.Data;
using workload.Models;
using workload.Models.ViewModels;

namespace workload.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> objList = _db.Department;
            return View(objList);
        }

        //Просмотр отчетов кафедры
        public IActionResult ViewReports(int? id)
        {
            //Просмотр видов работ текущей категории
            return View();
        }

        //Просмотр работников кафедры
        public IActionResult ViewWorkers(int? id)
        {
            return View();
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            Department obj = new Department();
            if (id == null)
            {
                return View(obj);
            }
            else
            {
                obj = _db.Department.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Department obj)
        {
            if (ModelState.IsValid)
            {
                if (obj == null || obj.Id == 0)
                {
                    _db.Department.Add(obj);
                }
                else
                {
                    _db.Department.Update(obj);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
