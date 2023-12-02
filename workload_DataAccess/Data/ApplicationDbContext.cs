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
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=50515051");
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
                new Degree("Кандидат")
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
                },
                new Department
                {
                    Id=3,
                    Name="Кафедра экономики"
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
                    NormHours=250,
                    AdditionalInfo="до 250 часов"
                },
                new ActivityType
                {
                    Id=2,
                    Name="Подготовка новой рабочей программы учебной дисциплины / программы дополнительного (профессионального) образования",
                    CategoryId=1,
                    NormHours=30,
                    AdditionalInfo="30 часов на программу"
                },
                new ActivityType {
                    Id=3,
                    Name="Обновление рабочих программ учебной дисциплины / программы дополнительного (профессионального) образования",
                    CategoryId=1,
                    NormHours=5,
                    AdditionalInfo="5 часов на программу"
                },
                new ActivityType
                {
                    Id=4,
                    Name="Подготовка новых методических разработок",
                    CategoryId=1,
                    NormHours=205
                },
                new ActivityType
                {
                    Id=5,
                    Name="Составление программы практики",
                    CategoryId=1,
                    NormHours=30,
                    AdditionalInfo="30 часов на программу"
                },
                new ActivityType
                {
                    Id=6,
                    Name="Обновление методических разработок",
                    CategoryId=1,
                    NormHours=5,
                    AdditionalInfo="5 часов на разработку"
                },
                new ActivityType
                {
                    Id=7,
                    Name="Подготовка к лекциям, семинарским, практическим и лабораторным занятиям с применением интерактивных форм обучения",
                    CategoryId=1,
                    NormHours=4,
                    AdditionalInfo="4 часа на каждый вид интерактивной формы"
                },
                new ActivityType
                {
                    Id=8,
                    Name="Подготовка конспектов лекций для впервые изучаемых дисциплин",
                    CategoryId=1,
                    NormHours=4,
                    AdditionalInfo="4 часа на лекцию"
                },
                new ActivityType
                {
                    Id=9,
                    Name="Подготовка к семинарским, практическим и лабораторным занятиям для впервые изучаемых дисциплин",
                    CategoryId=1,
                    NormHours=2,
                    AdditionalInfo="2 часа на занятие"
                },
                new ActivityType
                {
                    Id=10,
                    Name="Подготовка конспектов лекций к семинарским, практическим и лабораторным занятиям",
                    CategoryId=1,
                    NormHours=1,
                    AdditionalInfo="1 час на занятие"
                },
                new ActivityType
                {
                    Id=11,
                    Name="Полная актуализация комплекта учебно-методических материалов электронного курса для технологии дистанционного обучения",
                    CategoryId=1,
                    NormHours=60,
                    AdditionalInfo="60 часов на дисциплину"
                },
                new ActivityType
                {
                    Id=12,
                    Name="Прочие",
                    CategoryId=1,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=13,
                    Name="Работа в качестве секретаря совета факультета, заседаний кафедры",
                    CategoryId=2,
                    NormHours=60,
                    AdditionalInfo="60 часов в год"
                },
                new ActivityType
                {
                    Id=14,
                    Name="Участие в заседаниях кафедры и совета факультета",
                    CategoryId=2,
                    NormHours=20,
                    AdditionalInfo="20 часов в год"
                },
                new ActivityType
                {
                    Id=15,
                    Name="Работа в методическом совете университета / филиала",
                    CategoryId=2,
                    NormHours=60,
                    AdditionalInfo="до 60 часов"
                },
                new ActivityType
                {
                    Id=16,
                    Name="Ответственный за методическую работу по кафедре, факультету",
                    CategoryId=2,
                    NormHours=60,
                    AdditionalInfo="60 часов"
                },
                new ActivityType
                {
                    Id=17,
                    Name="Ответственный за работу в системе дистанционного обучения по кафедре",
                    CategoryId=2,
                    NormHours=100,
                    AdditionalInfo="100 часов"
                },
                new ActivityType
                {
                    Id=18,
                    Name="Выполнение обязанностей ответственного за контент сайта структурного подразделения университета",
                    CategoryId=2,
                    NormHours=30,
                    AdditionalInfo="30 часов в год"
                },
                new ActivityType
                {
                    Id=19,
                    Name="Взаимопосещение занятий преподавателями",
                    CategoryId=2,
                    NormHours=10,
                    AdditionalInfo="10 часов в год"
                },
                new ActivityType
                {
                    Id=20,
                    Name="Выполнение поручений по формированию банка тестовых заданий",
                    CategoryId=2,
                    NormHours=30,
                },
                new ActivityType
                {
                    Id=21,
                    Name="Выполнение поручений по формированию банка тестовых заданий",
                    CategoryId=2,
                    NormHours=30
                },
                new ActivityType
                {
                    Id=22,
                    Name="Выполнение поручений по организации производственной практики",
                    CategoryId=2,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=23,
                    Name="Выполнение поручений по организации распределения и выполнения ВКР",
                    CategoryId=2,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=24,
                    Name="Прочие",
                    CategoryId=2,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=25,
                    Name="Участие в заседаниях совета по науке",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=26,
                    Name="Выполнение исследований по НИР в соответствии с программой исследований (договором) с представлением отчёта, оформленного по ГОСТ 7.32.-2001",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=27,
                    Name="Подготовка диссертации согласно плану подготовки диссертации сотрудниками университета (указать выполнение глав)",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=28,
                    Name="Написание и подготовка к изданию монографии",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=29,
                    Name="Написание и подготовка к изданию научной статьи в журнале, входящем в базу Web of Science, Scopus",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=30,
                    Name="Написание и подготовка к изданию научной статьи в журнале из перечня ВАК, журнале \"Вестник СибУПК\"",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=31,
                    Name="Написание и подготовка к изданию научной статьи в сборнике конференций",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=32,
                    Name="Участие в научно-практических, научно-методических и других научных мероприятиях с подготовкой доклада (международных, национальных, межвузовских, университетских)",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=33,
                    Name="Руководство студенческим научным кружком с предоставлением протоколов заседаний кружков; руководство студенческим научно-инновационным проектом с предоставлением отчета о работе",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=34,
                    Name="Руководство НИРС (научные доклады, конкурсы, олимпиады, в т.ч. профориентационные)",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=35,
                    Name="Организация и проведение мастер-классов, деловых игр и др. в рамках научных инновационных форумов",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=36,
                    Name="Подготовка заявок на изобретение, конкурсы российских и международных грантов",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=37,
                    Name="Подготовка и проведение международных, российских и региональных научно-практических конференций (форумов, семинаров) на базе университета",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=38,
                    Name="Организация и проведение конкурсов (инновационных проектов, стендовых докладов)",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=39,
                    Name="Организация и проведение мероприятий, подготовка материалов (концепций, рекомендаций и т.п.) по плану взаимодействия с предприятиями потребительской кооперации, Межрегиональной ассоциацией \"Сибирское соглашение\" вузами, научными учреждениями",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=40,
                    Name="Подготовка отзыва на автореферат докторских и кандидатских диссертаций",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=41,
                    Name="Прочие",
                    CategoryId=3,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=42,
                    Name="Кураторство обучающихся 1 и 2 курсов с проведением еженедельных консультаций",
                    CategoryId=4,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=43,
                    Name="Классное руководство СПО, в том числе проведение классных часов",
                    CategoryId=4,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=44,
                    Name="Работа с потенциальными абитуриентами в школах, колледжах (с поступлением в университет), не менее 10 человек",
                    CategoryId=4,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=45,
                    Name="Организация и проведение мероприятий по воспитательной работе (на факультете, кафедре)",
                    CategoryId=4,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=46,
                    Name="Участие в мероприятиях для абитуриентов, проводимых на базе университета",
                    CategoryId=4,
                    NormHours=0
                },
                new ActivityType
                {
                    Id=47,
                    Name="Прочее",
                    CategoryId=4,
                    NormHours=0
                }
            };
            modelBuilder.Entity<ActivityType>().HasData(activityTypes);
        }
    }
}
