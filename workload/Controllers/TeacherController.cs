using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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
        private readonly UserManager<IdentityUser> _userManager;

        public TeacherController(ITeacherRepository teachRepo, UserManager<IdentityUser> userManager)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roleName = WC.TeacherName;
            var usersWIthRoles = _userManager.GetUsersInRoleAsync(roleName).Result.ToList();
            Teacher teacher = new Teacher();
            //Teacher<List> objlist = 
            return View(usersWIthRoles);
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
                if (teacherVM.Teacher == null)
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
