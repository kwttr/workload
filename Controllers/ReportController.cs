using Microsoft.AspNetCore.Mvc;
using workload.Data;
using workload.Models;

namespace workload.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Teacher> objList = _db.Teachers;
            return View(objList);
        }
    }
}