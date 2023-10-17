using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace workload_Models.ViewModels
{
    public class ActivityTypeVM
    {
        public ActivityType ActivityType { get; set; }
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
    }
}
