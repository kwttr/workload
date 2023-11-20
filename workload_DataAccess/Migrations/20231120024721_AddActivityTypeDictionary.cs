using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace workload_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityTypeDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "AdditionalInfo", "CategoryId", "Name", "NormHours" },
                values: new object[,]
                {
                    { 13, null, 2, "Работа в качестве секретаря совета факультета, заседаний кафедры", 0m },
                    { 14, null, 2, "Участие в заседаниях кафедры и совета факультета", 0m },
                    { 15, null, 2, "Работа в методическом совете университета / филиала", 0m },
                    { 16, null, 2, "Ответственный за методическую работу по кафедре, факультету", 0m },
                    { 17, null, 2, "Ответственный за работу в системе дистанционного обучения по кафедре", 0m },
                    { 18, null, 2, "Выполнение обязанностей ответственного за контент сайта структурного подразделения университета", 0m },
                    { 19, null, 2, "Взаимопосещение занятий преподавателями", 0m },
                    { 20, null, 2, "Выполнение поручений по формированию банка тестовых заданий", 0m },
                    { 21, null, 2, "Выполнение поручений по формированию банка тестовых заданий", 0m },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 47);
        }
    }
}
