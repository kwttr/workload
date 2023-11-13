using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Internal;
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
        private readonly RoleManager<CustomRole> _roleManager;

        public UserController(ITeacherRepository teachRepo, UserManager<IdentityUser> userManager, RoleManager<CustomRole> roleManager)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

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
                var roles = _roleManager.Roles.ToListAsync().Result;
                TeacherVM teacherVM = new TeacherVM()
                {
                    Teacher = new Teacher(),
                    DegreeSelectList = _teachRepo.GetAllDropdownList(WC.DegreeName),
                    PositionSelectList = _teachRepo.GetAllDropdownList(WC.PositionName),
                    DepartmentSelectList = _teachRepo.GetAllDropdownList(WC.DepartmentName),
                    RolesSelectList = roles
                };
                teacherVM.Teacher = _teachRepo.Find(id);
                teacherVM.SelectedRoles = _userManager.GetRolesAsync(teacherVM.Teacher).Result.ToList();
                if (teacherVM.Teacher == null)
                {
                    return NotFound();
                }
                return View(teacherVM);
            }
        }

        public async void AddRoleToUser(string id,Teacher user)
        {
            await _userManager.AddToRoleAsync(user, id);
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
                teacher.DegreeId = vm.Teacher.DegreeId;
                teacher.PositionId = vm.Teacher.PositionId;

                //РОЛИ
                var roles = _userManager.GetRolesAsync(vm.Teacher).Result;
                if (vm.SelectedRoles!=null)
                {
                    foreach (var role in roles)
                    {
                        if (!vm.SelectedRoles.Contains(role))
                        {
                            _userManager.RemoveFromRoleAsync(teacher, role);
                        }
                    }
                    foreach (var obj in vm.SelectedRoles)
                    {
                        AddRoleToUser(obj, teacher);
                    }
                }

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
