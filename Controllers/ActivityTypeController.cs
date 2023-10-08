using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using workload.Data;
using workload.Models;
using workload.Models.ViewModels;

namespace workload.Controllers
{
    public class ActivityTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ActivityTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ActivityType> objList = _db.Activities;

            foreach(var obj in objList)
            {
                obj.Category = _db.Categories.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            ActivityTypeVM activityTypeVM = new ActivityTypeVM()
            {
                ActivityType = new ActivityType(),
                CategorySelectList = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(activityTypeVM);
            }
            else
            {
                activityTypeVM.ActivityType = _db.Activities.Find(id);
                if(activityTypeVM.ActivityType == null)
                {
                    return NotFound();
                }
                return View(activityTypeVM);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ActivityTypeVM activityTypeVM)
        {
            if (ModelState.IsValid)
            {
                if(activityTypeVM.ActivityType == null) {
                _db.Activities.Add(activityTypeVM.ActivityType);
                }
                else
                {
                    //updating
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
