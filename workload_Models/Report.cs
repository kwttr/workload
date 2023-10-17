using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload_Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        //Display in list
        public string Title { get; set; }

        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher? Teacher { get; set; }

        public string? CurrentDegree { get; set; }
        public double? Rate { get; set; }

        //Display in form
        public List<ProcessActivityType>? ProcessActivities { get; set; }
    }
}