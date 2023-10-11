using Microsoft.AspNetCore.Mvc.Rendering;

namespace workload.Models.ViewModels
{
    public class TeacherVM
    {
        public Teacher Teacher { get; set; }
        public IEnumerable<SelectListItem>? DegreeSelectList { get; set; }
        public IEnumerable<SelectListItem>? PositionSelectList { get; set; }
    }
}
