using Microsoft.AspNetCore.Mvc;
using workload.Data;
using workload.Models;

namespace workload.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Categories;
            return View(objList);
        }
        
        public IActionResult ViewActivities(int? id)
        {
            //Просмотр видов работ текущей категории
            return View();
        }
    }
}
