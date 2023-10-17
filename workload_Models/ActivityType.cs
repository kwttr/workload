using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload_Models
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
        public ActivityType(string name, int normHours, int categoryId)
        {
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
        }

        public ActivityType(){ }
        public ActivityType(int id, string name, int normHours, int categoryId, Category? category)
        {
            Id = id;
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
            Category = category;
        }
    }
}