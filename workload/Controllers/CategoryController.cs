using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IActivityTypeRepository _activityTypeRepo;

        public CategoryController(ICategoryRepository catRepo, IActivityTypeRepository activityTypeRepo)
        {
            _catRepo = catRepo;
            _activityTypeRepo = activityTypeRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }
        
        public IActionResult ViewActivities(int? id)
        {
            IEnumerable<ActivityType> objList = _activityTypeRepo.GetAll(includeProperties: "Category").Where(x => x.CategoryId == id);
            return View(objList);
        }
    }
}
