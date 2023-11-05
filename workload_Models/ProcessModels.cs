﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using workload_Models.ModelBinders;

namespace workload_Models
{
    public class ProcessActivityType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        public decimal NormHours { get; set; }

        public string? DatePlan { get; set; }
        public string? DateFact { get; set; }

        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        public decimal HoursPlan { get; set; }
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        public decimal HoursFact { get; set; }

        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        public decimal UnitPlan { get; set; }
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        public decimal UnitFact { get; set; }

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