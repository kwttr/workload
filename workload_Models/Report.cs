using Microsoft.AspNetCore.Mvc;
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
        [Display(Name = "Название")]
        public string Title { get; set; }

        //Display in TitlePage
        [Display(Name = "Пользователь")]
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher? Teacher { get; set; }

        public string? CurrentDegree { get; set; }
        public string? CurrentPosition { get; set; }
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Ставка преподавателя")]
        public double? Rate { get; set; }

        public string hodName { get; set; }
        public string hodSecondName { get; set; }
        public string hodPatronymic { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        //Display in Table for an year
        [BindProperty(BinderType = typeof(CustomDoubleModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal totalWorkPlan { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal totalWorkFact { get; set; }
        //Первый семестр
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal septemberFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal octoberFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal novemberFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal decemberFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal januaryFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal surveyFirstSemester{ get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal firstSemesterPlan { get; set; }
        //Второй семестр
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal februaryFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal marchFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal aprilFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal mayFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal juneFact { get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal surveySecondSemester{ get; set; }
        [BindProperty(BinderType = typeof(CustomDecimalModelBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.##########}")]
        public decimal secondSemesterPlan { get; set; }
        //Display in form
        public List<ProcessActivityType>? ProcessActivities { get; set; }
        
        public int StatusId { get; set; }
        public Status? Status { get; set; }

        public Report()
        {
            totalWorkPlan = 0;
            totalWorkFact = 0;
            //Первый семестр
            septemberFact = 0;
            octoberFact = 0;
            novemberFact = 0;
            decemberFact = 0;
            januaryFact = 0;
            surveyFirstSemester = 0;
            firstSemesterPlan = 0;
            //Второй семестр
            februaryFact = 0;
            marchFact = 0;
            aprilFact = 0;
            mayFact = 0;
            juneFact = 0;
            surveySecondSemester = 0;
            secondSemesterPlan = 0;
        }
    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}