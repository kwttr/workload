using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload_Models
{
    public class Teacher : IdentityUser
    {
        public string FullName { get; set; }

        public int DegreeId { get; set; }
        public Degree? Degree{ get; set; }

        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public List<Report>? Reports { get; set; }
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
