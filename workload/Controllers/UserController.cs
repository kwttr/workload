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
using workload_DataAccess.Repository;
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
        private readonly ITeacherDepartmentRepository _teachDepRepo;

        public UserController(ITeacherRepository teachRepo, UserManager<IdentityUser> userManager, RoleManager<CustomRole> roleManager, ITeacherDepartmentRepository teachDepRepo)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _teachDepRepo = teachDepRepo;
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

                //РОЛИ И КАФЕДРА
                var roles = _userManager.GetRolesAsync(teacher).Result;
                if (vm.SelectedRoles != null)
                {
                    //РОЛИ
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

                    //КАФЕДРА
                    roles=_userManager.GetRolesAsync(teacher).Result;
                    List<TeacherDepartment> newTeachDep = new List<TeacherDepartment>();
                    foreach(var role in roles)
                    {
                        var depRole = _roleManager.FindByNameAsync(role).Result;
                        newTeachDep.Add(new TeacherDepartment() { DepartmentId = depRole.DepartmentId, TeacherId = teacher.Id });
                    }
                    var oldTeachDep = _teachDepRepo.GetAll(c => c.TeacherId == teacher.Id).ToList();
                    var removedItems = oldTeachDep.Where(oldDep => !newTeachDep.Any(newDep => newDep.DepartmentId == oldDep.DepartmentId)).ToList();
                    var count = oldTeachDep.RemoveAll(oldDep => !newTeachDep.Any(newDep => newDep.DepartmentId == oldDep.DepartmentId));
                    if(count > 0)
                    {
                        foreach (var item in removedItems)
                        {
                            _teachDepRepo.Remove(item);
                        }
                    }
                    newTeachDep = newTeachDep.GroupBy(x => x.DepartmentId).Select(g => g.First()).ToList();
                    foreach(var item in newTeachDep)
                    {
                        if (!oldTeachDep.Contains(item))
                        {
                            _teachDepRepo.Add(item);
                        }
                    }

                }
                _teachDepRepo.Save();
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
