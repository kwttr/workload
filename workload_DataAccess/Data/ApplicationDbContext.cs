using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using workload_Models;

namespace workload_Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser,CustomRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        public ApplicationDbContext() {
            Database.EnsureCreated();
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ActivityType> Activities { get; set; }
        public DbSet<ProcessActivityType> ProcessActivityType { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Degree> Degree { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TeacherDepartment> TeacherDepartment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomRole>()
                .HasOne(c => c.Department)
                .WithMany()
                .HasForeignKey(c => c.DepartmentId);

            modelBuilder.Entity<TeacherDepartment>()
                .HasKey(c => new { c.TeacherId, c.DepartmentId });

            modelBuilder.Entity<TeacherDepartment>()
                .HasOne<Teacher>(c => c.Teacher)
                .WithMany(d => d.TeacherDepartments)
                .HasForeignKey(c => c.TeacherId);

            modelBuilder.Entity<TeacherDepartment>()
                .HasOne<Department>(d => d.Department)
                .WithMany(d => d.TeacherDepartments)
                .HasForeignKey(c => c.DepartmentId);

            modelBuilder.Entity<CustomRole>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique(false);
                entity.HasIndex(e=>e.NormalizedName).IsUnique(false);
                entity.HasIndex(x => new { x.NormalizedName,x.DepartmentId}).IsUnique();
            });

            var categories = new Category[]{
                new Category{
                    Id = 1,
                    Name = "Учебно-методическая работа"
                },
                new Category{
                    Id= 2,
                    Name = "Организационно-методическая работа"
                },
                new Category
                {
                    Id= 3,
                    Name = "Научно-исследовательская работа"
                },
                new Category
                {
                    Id= 4,
                    Name = "Профориентационная и воспитательная работа"
                }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            var degrees = new Degree[]
            {
                new Degree
                {
                    Id=1,
                    Name = "Доцент"
                },
                new Degree
                {
                    Id=2,
                    Name = "Профессор"
                },
                new Degree
                {
                    Id=3,
                    Name = "Кандидат"
                },
                new Degree
                {
                    Id=4,
                    Name = "Доктор"
                }
            };
            modelBuilder.Entity<Degree>().HasData(degrees);

            var positions = new Position[]
            {
                new Position
                {
                    Id=1,
                    Name = "Аспирант"
                },
                new Position
                {
                    Id=2,
                    Name = "Ассистент"
                },
                new Position
                {
                    Id=3,
                    Name = "Ведущий научный сотрудник"
                },
                new Position
                {
                    Id=4,
                    Name = "Главный научный сотрудник"
                },
                new Position
                {
                    Id=5,
                    Name = "Преподаватель"
                }
            };
            modelBuilder.Entity<Position>().HasData(positions);

            var statuses = new Status[]
            {
                new Status
                {
                    Id=1,
                    Name="Назначен отчет"
                },
                new Status
                {
                    Id=2,
                    Name="Отправлен на проверку"
                },
                new Status
                {
                    Id=3,
                    Name="Подтверждено"
                }
            };
            modelBuilder.Entity<Status>().HasData(statuses);

            var departments = new Department[]
            {
                new Department
                {
                    Id=1,
                    Name = "Кафедра информатики"
                },
                new Department
                {
                    Id=2,
                    Name = "Кафедра математики"
                }
            };
            modelBuilder.Entity<Department>().HasData(departments);

            var activityTypes = new ActivityType[]
            {
                new ActivityType
                {
                    Id=1,
                    Name="Подготовка к изданию учебных пособий",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=2,
                    Name="Подготовка новой рабочей программы учебной дисциплины / программы дополнительного (профессионального) образования",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType {
                    Id=3,
                    Name="Обновление рабочих программ учебной дисциплины / программы дополнительного (профессионального) образования",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=4,
                    Name="Подготовка новых методических разработок",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=5,
                    Name="Составление программы практики",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=6,
                    Name="Обновление методических разработок",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=7,
                    Name="Подготовка к лекциям, семинарским, практическим и лабораторным занятиям с применением интерактивных форм обучения",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=8,
                    Name="Подготовка конспектов лекций для впервые изучаемых дисциплин",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=9,
                    Name="Подготовка к семинарским, практическим и лабораторным занятиям для впервые изучаемых дисциплин",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=10,
                    Name="Подготовка конспектов лекций к семинарским, практическим и лабораторным занятиям",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=11,
                    Name="Полная актуализация комплекта учебно-методических материалов электронного курса для технологии дистанционного обучения",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=12,
                    Name="Прочие",
                    CategoryId=1,
                    NormHours=0
                }
            };
            modelBuilder.Entity<ActivityType>().HasData(activityTypes);
        }
    }
}
