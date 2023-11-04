using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload_Models
{
    public class ProcessActivityType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public double NormHours { get; set; }

        public string? DatePlan { get; set; }
        public string? DateFact { get; set; }

        public double HoursPlan { get; set; }
        public double HoursFact { get; set; }

        public double UnitPlan { get; set; }
        public double UnitFact { get; set; }

        [Display(Name = "Report Id")]
        public int ReportId { get; set; }
        [ForeignKey("ReportId")]
        public Report? Report { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}