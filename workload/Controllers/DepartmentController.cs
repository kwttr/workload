using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _depRepo;

        public DepartmentController(IDepartmentRepository depRepo)
        {
            _depRepo = depRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> objList = _depRepo.GetAll();
            return View(objList);
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
                obj = _depRepo.Find(id.GetValueOrDefault());
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
                    _depRepo.Add(obj);
                }
                else
                {
                    _depRepo.Update(obj);
                }
                _depRepo.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
