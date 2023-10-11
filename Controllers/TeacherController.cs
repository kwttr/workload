using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using workload.Data;
using workload.Models;
using workload.Models.ViewModels;

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

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            TeacherVM teacherVM = new TeacherVM()
            {
                Teacher = new Teacher(),
                DegreeSelectList = _db.Degree.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                PositionSelectList = _db.Position.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(teacherVM);
            }
            else
            {
                teacherVM.Teacher= _db.Teachers.Find(id);
                if (teacherVM.Teacher == null)
                {
                    return NotFound();
                }
                return View(teacherVM);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TeacherVM teacherVM)
        {
            if (ModelState.IsValid)
            {
                if (teacherVM.Teacher == null || teacherVM.Teacher.Id == 0)
                {
                    _db.Teachers.Add(teacherVM.Teacher);
                }
                else
                {
                    _db.Teachers.Update(teacherVM.Teacher);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
