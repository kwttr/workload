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
        public decimal? totalWorkPlan { get; set; }
        public decimal? totalWorkFact { get; set; }
        //Первый семестр
        public decimal? firstSemesterFact { get; set; }
        public decimal? septemberFact { get; set; }
        public decimal? octoberFact { get; set; }
        public decimal? novemberFact { get; set; }
        public decimal? decemberFact { get; set; }
        public decimal? januaryFact { get; set; }
        public decimal? surveyFirstSemester{ get; set; }
        //Второй семестр
        public decimal? secondSemesterFact { get; set; }
        public decimal? februaryFact { get; set; }
        public decimal? marchFact { get; set; }
        public decimal? aprilFact { get; set; }
        public decimal? mayFact { get; set; }
        public decimal? juneFact { get; set; }
        public decimal? surveySecondSemester{ get; set; }
        //Display in form
        public List<ProcessActivityType>? ProcessActivities { get; set; }
        
        public int StatusId { get; set; }
        public Status? Status { get; set; }
    }
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}