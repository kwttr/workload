using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Models.ViewModels;
using workload_Utility;

namespace workload.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ActivityTypeController : Controller
    {
        private readonly IActivityTypeRepository _actRepo;

        public ActivityTypeController(IActivityTypeRepository actRepo)
        {
            _actRepo = actRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<ActivityType> objList = _actRepo.GetAll(includeProperties: "Category");
            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            ActivityTypeVM activityTypeVM = new ActivityTypeVM()
            {
                ActivityType = new ActivityType(),
                CategorySelectList = _actRepo.GetAllDropdownList(WC.CategoryName)
            };
            if (id == null)
            {
                return View(activityTypeVM);
            }
            else
            {
                activityTypeVM.ActivityType = _actRepo.Find(id.GetValueOrDefault());
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
                    _actRepo.Add(activityTypeVM.ActivityType);
                }
                else
                {
                    _actRepo.Update(activityTypeVM.ActivityType);
                }
                _actRepo.Save();
                return RedirectToAction("Index");
            }
            activityTypeVM.CategorySelectList = _actRepo.GetAllDropdownList(WC.CategoryName);
            return View(activityTypeVM);
        }

        //GET - DELETE
        public ActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var obj = _actRepo.Find(id.GetValueOrDefault());
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
            var obj = _actRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _actRepo.Remove(obj);
            _actRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
