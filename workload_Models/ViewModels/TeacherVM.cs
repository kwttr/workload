using Microsoft.AspNetCore.Mvc.Rendering;

namespace workload_Models.ViewModels
{
    public class TeacherVM
    {
        public Teacher Teacher { get; set; }
        public IEnumerable<SelectListItem>? DegreeSelectList { get; set; }
        public IEnumerable<SelectListItem>? PositionSelectList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentSelectList { get; set; }
    }
    public class TeacherProfileVM
    {
        public Teacher teacher { get; set; }
        public List<Report>? reportList { get; set; }
    }
}
