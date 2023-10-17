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

        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }
        
        public IActionResult ViewActivities(int? id)
        {
            //Просмотр видов работ текущей категории
            return View();
        }
    }
}
