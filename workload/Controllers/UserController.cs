using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;
using workload.Services;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;
using workload_Utility.ClaimTypes;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class UserController : Controller
    {
        private readonly ITeacherRepository _teachRepo;
        private readonly CustomUserManager<IdentityUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly ITeacherDepartmentRepository _teachDepRepo;
        private readonly IUsers _iUser;

        public UserController(ITeacherRepository teachRepo,
                              CustomUserManager<IdentityUser> userManager,
                              RoleManager<CustomRole> roleManager,
                              ITeacherDepartmentRepository teachDepRepo,
                              IUsers users)
        {
            _teachRepo = teachRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _teachDepRepo = teachDepRepo;
            _iUser = users;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            List<Teacher> objList = new List<Teacher>();
            foreach (var user in users)
            {
                objList.Add(_teachRepo.Find(user.Id));
            }
            return View(objList);
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
                TeacherVM teacherVm = new TeacherVM()
                {
                    Teacher = new Teacher(),
                    DegreeSelectList = _teachRepo.GetAllDropdownList(WC.DegreeName),
                    PositionSelectList = _teachRepo.GetAllDropdownList(WC.PositionName),
                    DepartmentSelectList = _teachRepo.GetAllDropdownList(WC.DepartmentName),
                    RolesSelectList = roles
                };
                teacherVm.Teacher = _teachRepo.Find(id);
                teacherVm.SelectedRoles = _userManager.GetRolesAsync(teacherVm.Teacher).Result.ToList();
                return View(teacherVm);
            }
        }

        public async Task AddRoleToUser(string id, Teacher user)
        {
            await _userManager.AddToRoleAsync(user, id);
        }

        public async Task RemoveClaim(string id, string role)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            CustomRole tempRole = _roleManager.FindByNameAsync(role).Result;
            string roleaccess = Regex.Replace(role, @"\d", "");
            CustomClaimValue claim = new CustomClaimValue() { DepartmentId = tempRole.DepartmentId.ToString(), RoleAccess = roleaccess };
            var userClaim = _userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == "UserRoleDep" && c.Value == JsonConvert.SerializeObject(claim));
            if (userClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, userClaim);
            }
        }

        public async Task AddClaim(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            CustomRole tempRole = await _roleManager.FindByNameAsync(role);
            string roleaccess = Regex.Replace(role, @"\d", "");
            CustomClaimValue claim = new CustomClaimValue() { DepartmentId = tempRole.DepartmentId.ToString(), RoleAccess = roleaccess };
            var userClaim = _userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == "UserRoleDep" && c.Value == JsonConvert.SerializeObject(claim));
            if (userClaim == null)
            {
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimType.UserRoleDep, JsonConvert.SerializeObject(claim)));
            }
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(TeacherVM vm)
        {
            if (ModelState.IsValid)
            {
                //КОСТЫЛЬ!!!!!!!
                Teacher teacher = _teachRepo.Find(vm.Teacher.Id);
                teacher.FirstName = vm.Teacher.FirstName;
                teacher.LastName = vm.Teacher.LastName;
                teacher.Patronymic = vm.Teacher.Patronymic;
                teacher.DegreeId = vm.Teacher.DegreeId;
                teacher.PositionId = vm.Teacher.PositionId;

                //РОЛИ И КАФЕДРА
                var roles = await _userManager.GetRolesAsync(teacher);
                if (vm.SelectedRoles != null)
                {
                    //РОЛИ
                    foreach (var role in roles)
                    {
                        if (!vm.SelectedRoles.Contains(role))
                        {
                            await _userManager.RemoveFromRoleAsync(teacher, role);
                            await RemoveClaim(teacher.Id, role);
                        }
                    }
                    foreach (var obj in vm.SelectedRoles)
                    {
                        await AddRoleToUser(obj, teacher);
                        await AddClaim(teacher.Id, obj);
                    }

                    //КАФЕДРА
                    List<TeacherDepartment> newTeachDep = new List<TeacherDepartment>();
                    foreach (var role in vm.SelectedRoles)
                    {
                        var depRole = await _roleManager.FindByNameAsync(role);
                        newTeachDep.Add(new TeacherDepartment() { DepartmentId = depRole.DepartmentId, TeacherId = teacher.Id });
                    }
                    var oldTeachDep = _teachDepRepo.GetAll(c => c.TeacherId == teacher.Id).ToList();
                    var removedItems = oldTeachDep.Where(oldDep => !newTeachDep.Any(newDep => newDep.DepartmentId == oldDep.DepartmentId)).ToList();
                    var count = oldTeachDep.RemoveAll(oldDep => !newTeachDep.Any(newDep => newDep.DepartmentId == oldDep.DepartmentId));
                    if (count > 0)
                    {
                        foreach (var item in removedItems)
                        {
                            _teachDepRepo.Remove(item);
                        }
                    }
                    newTeachDep = newTeachDep.GroupBy(x => x.DepartmentId).Select(g => g.First()).ToList();
                    foreach (var item in newTeachDep)
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
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _teachRepo.Find(id);
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(string? id)
        {
            if (id != null)
            {
                var obj = _teachRepo.Find(id);
                _teachRepo.Remove(obj);
            }

            _teachRepo.Save();
            return RedirectToAction("Index");
        }

        //GET - BLOCK
        public IActionResult Block(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _teachRepo.Find(id);
            return View(obj);
        }

        //POST - BLOCK
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Block(Teacher obj)
        {
            obj = _teachRepo.Find(obj.Id);
            _iUser.LockUser(obj.Email, new DateTime(2222, 06, 06));
            return RedirectToAction("Index");
        }

        //GET - CHANGE PASSWORD
        public IActionResult ChangePassword(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ChangePasswordVM vm = new ChangePasswordVM() { TeacherId =  id };
            return View(vm);
        }

        //POST - CHANGE PASSWORD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm.TeacherId);
            }

            var user = await _userManager.FindByIdAsync(vm.TeacherId);
            if (user == null)
            {
                return NotFound("Unable to load user with this ID.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, vm.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }
            _iUser.UnlockUser(user.Email);
            return RedirectToAction("Index");
        }
    }
}
