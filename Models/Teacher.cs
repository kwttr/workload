using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int DegreeId { get; set; }
        public Degree? Degree{ get; set; }

        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public List<Report>? Reports { get; set; }

        [Display(Name="Department Name")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
    }
    public class Degree
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
