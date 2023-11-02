using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Collections.Immutable;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class UserController : Controller
    {
        private readonly ITeacherRepository _teachRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ITeacherRepository teachRepo, UserManager<IdentityUser> userManager)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            //var roleName = WC.TeacherName;
            //var usersWIthRoles = _userManager.GetUsersInRoleAsync(roleName).Result.ToList();
            //List<Teacher> objlist = new List<Teacher>();
            //foreach(var user in usersWIthRoles)
            //{
            //    objlist.Add(_teachRepo.Find(user.Id));
            //}
            return View(users);
        }

        //GET - EDIT
        public IActionResult Edit(string? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                TeacherVM teacherVM = new TeacherVM()
                {
                    Teacher = new Teacher(),
                    DegreeSelectList = _teachRepo.GetAllDropdownList(WC.DegreeName),
                    PositionSelectList = _teachRepo.GetAllDropdownList(WC.PositionName),
                    DepartmentSelectList = _teachRepo.GetAllDropdownList(WC.DepartmentName)
                };
                teacherVM.Teacher = _teachRepo.Find(id);
                if (teacherVM.Teacher == null)
                {
                    return NotFound();
                }
                return View(teacherVM);
            }
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherVM vm)
        {
            if(ModelState.IsValid)
            {
                //КОСТЫЛЬ!!!!!!!
                Teacher teacher = _teachRepo.Find(vm.Teacher.Id);
                teacher.FullName = vm.Teacher.FullName;
                teacher.DepartmentId = vm.Teacher.DepartmentId;
                teacher.DegreeId = vm.Teacher.DegreeId;
                teacher.PositionId = vm.Teacher.PositionId;
                _teachRepo.Update(teacher);
                _teachRepo.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        //GET - DELETE
        public ActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _teachRepo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(string? id)
        {
            var obj = _teachRepo.Find(id);
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
