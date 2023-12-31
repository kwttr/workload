﻿using Microsoft.AspNetCore.Mvc;
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
        [Display(Name = "Название")]
        public string Name { get; set; }

        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        [Display(Name = "Норма часов")]
        public decimal NormHours { get; set; }

        [Display(Name="Вид категории")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Display(Name="Дополнительная информация")]
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