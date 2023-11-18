﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using workload_Models.ModelBinders;

namespace workload_Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        //Display in list
        public string Title { get; set; }

        [Display(Name = "Teacher")]
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher? Teacher { get; set; }

        public string? CurrentDegree { get; set; }
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Ставка преподавателя")]
        public double? Rate { get; set; }

        public int StatusId { get; set; }
        public Status? Status { get; set; }

        //Display in form
        public List<ProcessActivityType>? ProcessActivities { get; set; }
    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}