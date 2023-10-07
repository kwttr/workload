using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workload.Data;
using workload.Models;

namespace workload.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TeacherController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Teacher> objList = _db.Teachers;
            return View(objList);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher obj) 
        {
            if (ModelState.IsValid)
            {
                _db.Teachers.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - EDIT
        public ActionResult Edit(int? id) 
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }
            var obj = _db.Teachers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher obj)
        {
            if (ModelState.IsValid)
            {
                _db.Teachers.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET - DELETE
        public ActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var obj = _db.Teachers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
            var obj = _db.Teachers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Teachers.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
