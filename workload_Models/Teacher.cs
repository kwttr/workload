﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace workload_Models
{
    public class Teacher : IdentityUser
    {
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Научная степень")]
        public int DegreeId { get; set; }
        public Degree? Degree { get; set; }

        [Display(Name = "Должность")]
        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public List<TeacherDepartment>? TeacherDepartments { get; set; }

        public List<Report>? Reports { get; set; }
    }

    public class Degree
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }

    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }

    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public List<TeacherDepartment>? TeacherDepartments { get; set; }
    }

    public class TeacherDepartment
    {
        public string TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is TeacherDepartment other)
            {
                return this.DepartmentId == other.DepartmentId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
