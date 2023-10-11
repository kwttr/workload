using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload.Models
{
    public class ProcessActivityType
    {
        [Key]
        public int Id { get; set; }

        public string? DatePlan { get; set; }
        public string? DateFact { get; set; }

        public int HoursPlan { get; set; }
        public int HoursFact { get; set; }

        public int UnitPlan { get; set; }
        public int UnitFact { get; set; }

        public int ActivityTypeId { get; set; }
        [ForeignKey(nameof(ActivityTypeId))]
        public ActivityType ActivityType { get; set; }

        [Display(Name = "Report Id")]
        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public Report? Report { get; set; }

    }
}