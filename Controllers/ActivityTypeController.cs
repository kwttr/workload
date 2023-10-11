using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                if(activityTypeVM.ActivityType == null || activityTypeVM.ActivityType.Id == 0) {
                    _db.Activities.Add(activityTypeVM.ActivityType);
                }
                else
                {
                    _db.Activities.Update(activityTypeVM.ActivityType);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //GET - DELETE
        public ActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var obj = _db.Activities.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
            var obj = _db.Activities.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Activities.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
