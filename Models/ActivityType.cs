using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload.Models
{
    public class ActivityType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int NormHours { get; set; }
        [Display(Name="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}