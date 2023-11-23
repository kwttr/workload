using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _depRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly ITeacherDepartmentRepository _teacherDepartmentRepository;
        private readonly RoleManager<CustomRole> _roleManager;

        public DepartmentController(IDepartmentRepository depRepo, ITeacherRepository teacherRepo, RoleManager<CustomRole> roleManager, ITeacherDepartmentRepository teacherDepartmentRepository)
        {
            _depRepo = depRepo;
            _teacherRepo = teacherRepo;
            _roleManager = roleManager;
            _teacherDepartmentRepository = teacherDepartmentRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> objList = _depRepo.GetAll();
            return View(objList);
        }

        //Просмотр работников кафедры
        public IActionResult ViewWorkers(int? id)
        {
            IEnumerable<TeacherDepartment> teachdeps = _teacherDepartmentRepository.GetAll(x=>x.DepartmentId==id);
            List<Teacher> objList = new List<Teacher>();
            foreach(var teach in teachdeps)
            {
                objList.Add(_teacherRepo.Find(teach.TeacherId));
            }
            return View(objList);
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

        public async void CreateRoles(int id)
        {
            _roleManager.RoleValidators.Clear();
            _roleManager.RoleValidators.Add(new MyRoleValidator());
            await _roleManager.CreateAsync(new CustomRole
            {
                Name = WC.HeadOfDepartmentRole+id,
                DepartmentId = id
            });
            await _roleManager.CreateAsync(new CustomRole
            {
                Name = WC.TeacherRole+id,
                DepartmentId = id
            });
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
                CreateRoles(obj.Id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
