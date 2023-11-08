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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal NormHours { get; set; }

        [Display(Name="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [Display(Name="Additional Info")]

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public string? AdditionalInfo { get; set; }

        public ActivityType(string name, decimal normHours, int categoryId)
        {
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
        }

        public ActivityType(){ }
        public ActivityType(int id, string name, decimal normHours, int categoryId, Category? category)
        {
            Id = id;
            Name = name;
            NormHours = normHours;
            CategoryId = categoryId;
            Category = category;
        }
    }
}