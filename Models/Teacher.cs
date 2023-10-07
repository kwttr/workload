using System.ComponentModel.DataAnnotations;

namespace workload.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Degree { get; set; }
    }
}
