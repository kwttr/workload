using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using workload_Models.ModelBinders;

namespace workload_Models
{
    public class ActivityType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))] 
        public double NormHours { get; set; }
        [Display(Name="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public ActivityType(string name, double normHours, int categoryId)
        {
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
        }

        public ActivityType(){ }
        public ActivityType(int id, string name, double normHours, int categoryId, Category? category)
        {
            Id = id;
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
            Category = category;
        }
    }
}