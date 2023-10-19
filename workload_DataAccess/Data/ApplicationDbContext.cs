using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using workload_Models;

namespace workload_Data
{
    public class ApplicationDbContext : IdentityDbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
