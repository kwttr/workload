using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace workload_DataAccess.Migrations
{
    public partial class toPostgresql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degree", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormHours = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    DegreeId = table.Column<int>(type: "integer", nullable: true),
                    PositionId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Degree_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<string>(type: "text", nullable: false),
                    CurrentDegree = table.Column<string>(type: "text", nullable: true),
                    CurrentPosition = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<double>(type: "double precision", nullable: true),
                    hodName = table.Column<string>(type: "text", nullable: false),
                    hodSecondName = table.Column<string>(type: "text", nullable: false),
                    hodPatronymic = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    totalWorkPlan = table.Column<decimal>(type: "numeric", nullable: false),
                    totalWorkFact = table.Column<decimal>(type: "numeric", nullable: false),
                    septemberFact = table.Column<decimal>(type: "numeric", nullable: false),
                    octoberFact = table.Column<decimal>(type: "numeric", nullable: false),
                    novemberFact = table.Column<decimal>(type: "numeric", nullable: false),
                    decemberFact = table.Column<decimal>(type: "numeric", nullable: false),
                    januaryFact = table.Column<decimal>(type: "numeric", nullable: false),
                    surveyFirstSemester = table.Column<decimal>(type: "numeric", nullable: false),
                    firstSemesterPlan = table.Column<decimal>(type: "numeric", nullable: false),
                    februaryFact = table.Column<decimal>(type: "numeric", nullable: false),
                    marchFact = table.Column<decimal>(type: "numeric", nullable: false),
                    aprilFact = table.Column<decimal>(type: "numeric", nullable: false),
                    mayFact = table.Column<decimal>(type: "numeric", nullable: false),
                    juneFact = table.Column<decimal>(type: "numeric", nullable: false),
                    surveySecondSemester = table.Column<decimal>(type: "numeric", nullable: false),
                    secondSemesterPlan = table.Column<decimal>(type: "numeric", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherDepartment",
                columns: table => new
                {
                    TeacherId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDepartment", x => new { x.TeacherId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_TeacherDepartment_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherDepartment_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessActivityType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormHours = table.Column<decimal>(type: "numeric", nullable: false),
                    DatePlan = table.Column<string>(type: "text", nullable: true),
                    DateFact = table.Column<string>(type: "text", nullable: true),
                    HoursPlan = table.Column<decimal>(type: "numeric", nullable: false),
                    HoursFact = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPlan = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitFact = table.Column<decimal>(type: "numeric", nullable: false),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessActivityType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessActivityType_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessActivityType_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Учебно-методическая работа" },
                    { 2, "Организационно-методическая работа" },
                    { 3, "Научно-исследовательская работа" },
                    { 4, "Профориентационная и воспитательная работа" }
                });

            migrationBuilder.InsertData(
                table: "Degree",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Доцент" },
                    { 2, "Профессор" },
                    { 3, "Кандидат" },
                    { 4, "Доктор" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Кафедра информатики" },
                    { 2, "Кафедра математики" },
                    { 3, "Кафедра экономики" }
                });

            migrationBuilder.InsertData(
                table: "Position",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Аспирант" },
                    { 2, "Ассистент" },
                    { 3, "Ведущий научный сотрудник" },
                    { 4, "Главный научный сотрудник" },
                    { 5, "Преподаватель" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Назначен отчет" },
                    { 2, "Отправлен на проверку" },
                    { 3, "Подтверждено" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "AdditionalInfo", "CategoryId", "Name", "NormHours" },
                values: new object[,]
                {
                    { 1, "до 250 часов", 1, "Подготовка к изданию учебных пособий", 250m },
                    { 2, "30 часов на программу", 1, "Подготовка новой рабочей программы учебной дисциплины / программы дополнительного (профессионального) образования", 30m },
                    { 3, "5 часов на программу", 1, "Обновление рабочих программ учебной дисциплины / программы дополнительного (профессионального) образования", 5m },
                    { 4, null, 1, "Подготовка новых методических разработок", 205m },
                    { 5, "30 часов на программу", 1, "Составление программы практики", 30m },
                    { 6, "5 часов на разработку", 1, "Обновление методических разработок", 5m },
                    { 7, "4 часа на каждый вид интерактивной формы", 1, "Подготовка к лекциям, семинарским, практическим и лабораторным занятиям с применением интерактивных форм обучения", 4m },
                    { 8, "4 часа на лекцию", 1, "Подготовка конспектов лекций для впервые изучаемых дисциплин", 4m },
                    { 9, "2 часа на занятие", 1, "Подготовка к семинарским, практическим и лабораторным занятиям для впервые изучаемых дисциплин", 2m },
                    { 10, "1 час на занятие", 1, "Подготовка конспектов лекций к семинарским, практическим и лабораторным занятиям", 1m },
                    { 11, "60 часов на дисциплину", 1, "Полная актуализация комплекта учебно-методических материалов электронного курса для технологии дистанционного обучения", 60m },
                    { 12, null, 1, "Прочие", 0m },
                    { 13, "60 часов в год", 2, "Работа в качестве секретаря совета факультета, заседаний кафедры", 60m },
                    { 14, "20 часов в год", 2, "Участие в заседаниях кафедры и совета факультета", 20m },
                    { 15, "до 60 часов", 2, "Работа в методическом совете университета / филиала", 60m },
                    { 16, "60 часов", 2, "Ответственный за методическую работу по кафедре, факультету", 60m },
                    { 17, "100 часов", 2, "Ответственный за работу в системе дистанционного обучения по кафедре", 100m },
                    { 18, "30 часов в год", 2, "Выполнение обязанностей ответственного за контент сайта структурного подразделения университета", 30m },
                    { 19, "10 часов в год", 2, "Взаимопосещение занятий преподавателями", 10m },
                    { 20, null, 2, "Выполнение поручений по формированию банка тестовых заданий", 30m },
                    { 21, null, 2, "Выполнение поручений по формированию банка тестовых заданий", 30m },
                    { 22, null, 2, "Выполнение поручений по организации производственной практики", 0m },
                    { 23, null, 2, "Выполнение поручений по организации распределения и выполнения ВКР", 0m },
                    { 24, null, 2, "Прочие", 0m },
                    { 25, null, 3, "Участие в заседаниях совета по науке", 0m },
                    { 26, null, 3, "Выполнение исследований по НИР в соответствии с программой исследований (договором) с представлением отчёта, оформленного по ГОСТ 7.32.-2001", 0m },
                    { 27, null, 3, "Подготовка диссертации согласно плану подготовки диссертации сотрудниками университета (указать выполнение глав)", 0m },
                    { 28, null, 3, "Написание и подготовка к изданию монографии", 0m },
                    { 29, null, 3, "Написание и подготовка к изданию научной статьи в журнале, входящем в базу Web of Science, Scopus", 0m },
                    { 30, null, 3, "Написание и подготовка к изданию научной статьи в журнале из перечня ВАК, журнале \"Вестник СибУПК\"", 0m },
                    { 31, null, 3, "Написание и подготовка к изданию научной статьи в сборнике конференций", 0m },
                    { 32, null, 3, "Участие в научно-практических, научно-методических и других научных мероприятиях с подготовкой доклада (международных, национальных, межвузовских, университетских)", 0m },
                    { 33, null, 3, "Руководство студенческим научным кружком с предоставлением протоколов заседаний кружков; руководство студенческим научно-инновационным проектом с предоставлением отчета о работе", 0m },
                    { 34, null, 3, "Руководство НИРС (научные доклады, конкурсы, олимпиады, в т.ч. профориентационные)", 0m },
                    { 35, null, 3, "Организация и проведение мастер-классов, деловых игр и др. в рамках научных инновационных форумов", 0m },
                    { 36, null, 3, "Подготовка заявок на изобретение, конкурсы российских и международных грантов", 0m },
                    { 37, null, 3, "Подготовка и проведение международных, российских и региональных научно-практических конференций (форумов, семинаров) на базе университета", 0m },
                    { 38, null, 3, "Организация и проведение конкурсов (инновационных проектов, стендовых докладов)", 0m },
                    { 39, null, 3, "Организация и проведение мероприятий, подготовка материалов (концепций, рекомендаций и т.п.) по плану взаимодействия с предприятиями потребительской кооперации, Межрегиональной ассоциацией \"Сибирское соглашение\" вузами, научными учреждениями", 0m },
                    { 40, null, 3, "Подготовка отзыва на автореферат докторских и кандидатских диссертаций", 0m },
                    { 41, null, 3, "Прочие", 0m },
                    { 42, null, 4, "Кураторство обучающихся 1 и 2 курсов с проведением еженедельных консультаций", 0m },
                    { 43, null, 4, "Классное руководство СПО, в том числе проведение классных часов", 0m },
                    { 44, null, 4, "Работа с потенциальными абитуриентами в школах, колледжах (с поступлением в университет), не менее 10 человек", 0m },
                    { 45, null, 4, "Организация и проведение мероприятий по воспитательной работе (на факультете, кафедре)", 0m },
                    { 46, null, 4, "Участие в мероприятиях для абитуриентов, проводимых на базе университета", 0m },
                    { 47, null, 4, "Прочее", 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CategoryId",
                table: "Activities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_DepartmentId",
                table: "AspNetRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Name",
                table: "AspNetRoles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_NormalizedName_DepartmentId",
                table: "AspNetRoles",
                columns: new[] { "NormalizedName", "DepartmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DegreeId",
                table: "AspNetUsers",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessActivityType_CategoryId",
                table: "ProcessActivityType",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessActivityType_ReportId",
                table: "ProcessActivityType",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DepartmentId",
                table: "Reports",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StatusId",
                table: "Reports",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TeacherId",
                table: "Reports",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDepartment_DepartmentId",
                table: "TeacherDepartment",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProcessActivityType");

            migrationBuilder.DropTable(
                name: "TeacherDepartment");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Position");
        }
    }
}
