using Microsoft.AspNetCore.Mvc.Rendering;

namespace workload_Models.ViewModels
{
    public class ReportVM
    {
        public Report report { get; set; }
        public Teacher? teacher { get; set; }
        public IEnumerable<SelectListItem>? TeacherSelectList { get; set; }
    }
    public class ReportDetailsVM
    {
        public Report Report { get; set; }
        public List<ProcessActivityType> ProcessActivityTypes { get; set; }
        public List<Category> CategoryList { get; set; }
    }
}
