using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Utility;

namespace workload_DataAccess.Repository
{
    public class ActivityTypeRepository : Repository<ActivityType>, IActivityTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ActivityTypeRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(ActivityType obj)
        {
            _db.Activities.Update(obj);
        }
        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.CategoryName)
            {
                return _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }
    }
}
