using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teachRepo;

        public TeacherController(ITeacherRepository teachRepo)
        {
            _teachRepo = teachRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Teacher> objList = _teachRepo.GetAll();
            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            TeacherVM teacherVM = new TeacherVM()
            {
                Teacher = new Teacher(),
                DegreeSelectList = _teachRepo.GetAllDropdownList(WC.DegreeName),
                PositionSelectList = _teachRepo.GetAllDropdownList(WC.PositionName)
            };
            if (id == null)
            {
                return View(teacherVM);
            }
            else
            {
                teacherVM.Teacher= _teachRepo.Find(id.GetValueOrDefault());
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
                    _teachRepo.Add(teacherVM.Teacher);
                }
                else
                {
                    _teachRepo.Update(teacherVM.Teacher);
                }
                _teachRepo.Save();
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
            var obj = _teachRepo.Find(id.GetValueOrDefault());
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
            var obj = _teachRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _teachRepo.Remove(obj);
            _teachRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
