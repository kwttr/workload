using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload.Models
{
    public class HeadOfDepartment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Department Name")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}
