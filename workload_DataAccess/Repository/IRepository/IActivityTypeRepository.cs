using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workload_Models;

namespace workload_DataAccess.Repository.IRepository
{
    public interface IActivityTypeRepository : IRepository<ActivityType>
    {
        void Update(ActivityType obj);
        
        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
