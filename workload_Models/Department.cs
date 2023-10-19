using System.ComponentModel.DataAnnotations;

namespace workload_Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<Teacher>? Teachers { get; set; }
    }
}
