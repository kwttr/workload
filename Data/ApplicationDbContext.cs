using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using workload.Models;

namespace workload.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        public ApplicationDbContext() { 
            Database.EnsureCreated();
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ActivityType> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<HeadOfDepartment> HeadOfDepartments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
