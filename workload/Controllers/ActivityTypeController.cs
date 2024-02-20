using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            ActivityTypeVM activityTypeVm = new ActivityTypeVM()
            {
                ActivityType = new ActivityType(),
                CategorySelectList = _actRepo.GetAllDropdownList(WC.CategoryName)
            };
            if (id == null)
            {
                return View(activityTypeVm);
            }
            else
            {
                activityTypeVm.ActivityType = _actRepo.Find(id.GetValueOrDefault());
                return View(activityTypeVm);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ActivityTypeVM activityTypeVm)
        {
            if (ModelState.IsValid)
            {
                if(activityTypeVm.ActivityType.Id == 0) {
                    _actRepo.Add(activityTypeVm.ActivityType);
                }
                else
                {
                    _actRepo.Update(activityTypeVm.ActivityType);
                }
                _actRepo.Save();
                return RedirectToAction("Index");
            }
            activityTypeVm.CategorySelectList = _actRepo.GetAllDropdownList(WC.CategoryName);
            return View(activityTypeVm);
        }

        //GET - DELETE
        public ActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var obj = _actRepo.Find(id.GetValueOrDefault());

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
            var obj = _actRepo.Find(id.GetValueOrDefault());
            _actRepo.Remove(obj);
            _actRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
