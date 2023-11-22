using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Utility;
namespace workload_DataAccess.Repository
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _catRepo;

        public ReportRepository(ApplicationDbContext db, ICategoryRepository catRepo) : base(db)
        {
            _db = db;
            _catRepo = catRepo;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.TeacherName)
            {
                return _db.Teachers.Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Report obj)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Rate = obj.Rate;
                objFromDb.ProcessActivities = obj.ProcessActivities;
            }
        }

        //Надо будет вынести в отдельный файл, ибо к репозиторию это отношение не имеет.
        #region Export
        public MemoryStream Export(Report obj)
        {
            using (var stream = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
                {
                    //Создание основного содержимого документа
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    body = CreateTitlePage(body, obj);

                    Table titleTable = CreateTitleTable();
                    body.Append(titleTable);

                    Paragraph signature = new Paragraph();
                    signature.Append(new Run(new Text("Подпись преподавателя _________________________")));
                    signature.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "250" });
                    body.Append(signature);

                    //Разрыв страницы
                    Paragraph breakParagraph = new Paragraph();
                    Run run = new Run();
                    run.Append(new Break() { Type = BreakValues.Page });
                    breakParagraph.Append(run);
                    body.Append(breakParagraph);

                    //Создаются таблицы на каждую категорию
                    var categories = _catRepo.GetAll().ToList();
                    for (int i = 1; i < 5; i++)
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Append(new Run(new Text(i.ToString()+" " + categories[i-1].Name)));
                        paragraph.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "250", After = "100" });
                        paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                        body.Append(paragraph);

                        //Добавление текста в документ
                        Table table = CreateTable(obj.ProcessActivities.Where(pa=>pa.CategoryId==i).ToList());
                        body.Append(table);
                    }

                    mainPart.Document.Save();
                }

                return stream;
            }
        }

        //Создание титульника
        public static Body CreateTitlePage(Body body, Report obj)
        {
            Paragraph paragraph1 = new Paragraph();
            paragraph1.Append(new Run(new Text("УТВЕРЖДАЮ \nЗав.Кафедрой _______________________\n" +
                "______________ ____________________")));
            Indentation indentation = new Indentation() { Left = "4500" };
            paragraph1.ParagraphProperties = new ParagraphProperties(indentation);
            body.Append(paragraph1);

            Paragraph paragraph2 = new Paragraph();
            paragraph2.Append(new Run(new Text("(подпись)                      (И.О.Фамилия)")));
            Indentation indentation2 = new Indentation() { Left = "4500" };
            paragraph2.ParagraphProperties = new ParagraphProperties(indentation2);
            body.Append(paragraph2);

            Paragraph paragraph3 = new Paragraph();
            paragraph3.Append(new Run(new Text("<_____>_____________________ 20____г.")));
            Indentation indentation3 = new Indentation() { Left = "4500" };
            paragraph3.ParagraphProperties = new ParagraphProperties(indentation3);
            body.Append(paragraph3);

            Paragraph paragraph4 = new Paragraph();
            paragraph4.Append(new Run(new Text("ИНДИВИДУАЛЬНЫЙ ПЛАН-ОТЧЁТ РАБОТЫ ПРЕПОДАВАТЕЛЯ")));
            paragraph4.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
            body.Append(paragraph4);

            Paragraph paragraph5 = new Paragraph();
            paragraph5.Append(new Run(new Text("на __________________ учебный год")));
            paragraph5.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
            body.Append(paragraph5);
            
            Paragraph paragraphTeacherInfo = new Paragraph();
            paragraphTeacherInfo.Append(new Run(new Text("Фамилия ______________________________________" + 
                "\nИмя ___________ Отчество _______________" +
                "\nУченая степень, учёное звание ________________"+
                "\nКафедра__________________"+
                "\nДолжность____________________________"+
                "\nДата избрания на должность______________________________")));
            body.Append(paragraphTeacherInfo);

            Paragraph paragraph6 = new Paragraph();
            paragraph6.Append(new Run(new Text("Объем выполняемой работы за учебный год")));
            paragraph6.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "150", After = "150" });
            paragraph6.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
            body.Append(paragraph6);

            

            return body;
        }

        public Table CreateTitleTable()
        {
            //Заголовок
            Table table = new Table();

            for (int i = 0; i < 2; i++)
            {
                TableRow row = new TableRow();
                for (int j = 0; j < 5; j++)
                {
                    TableCell cell = new TableCell();

                    //Добавляем текст в ячейку
                    string cellText = GetHeaderTextTitlePage(i, j);
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    paragraph.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "0", After = "0" });
                    paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                    cell.Append(paragraph);

                    var props = new TableCellProperties();
                    props.Append(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
                    cell.Append(props);

                    if ((i == 0 && j == 0) || (i == 0 && j == 1) || (i == 0 && j == 4)) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if ((i == 1 && j == 0) || (i == 1 && j == 1) || (i == 1 && j == 4)) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 0 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    if (i == 0 && j == 3) continue;
                    row.Append(cell);
                }
                table.Append(row);
            }

            //Тело таблицы
            for (int i = 0; i < 22; i++)
            {
                TableRow row = new TableRow();
                for (int j = 0; j < 5; j++)
                {
                    TableCell cell = new TableCell();

                    //Добавляем текст в ячейку
                    string cellText = GetMainBodyText(i, j);
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    paragraph.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { Before = "0", After = "0" });
                    if (j == 0) paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                    if ((i == 2 || i == 10) && j == 2) paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                    if ((i == 9 || i == 17) && j == 1) paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Right });
                    cell.Append(paragraph);

                    if (i == 0 && j == 0) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i > 0 && i < 18 && j==0) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));


                    if (i == 1 && j == 1) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 4 }));
                    if (i == 2 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    if (i == 3 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i > 3 && i < 10 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));
                    if (i == 10 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    if (i == 11 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i > 11 && i < 18 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if ((i == 1 && j > 1) || (i == 2 && j == 3) || (i == 10 && j == 3)) continue;

                    row.Append(cell);
                }
                table.Append(row);
            }
            // Добавляем границы таблицы
            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                )
            );
            table.Append(tableProperties);
            return table;
        }

        static string GetHeaderTextTitlePage(int row, int col)
        {
            // Возвращает текст для ячейки таблицы в зависимости от её положения
            if (row == 0)
            {
                switch (col)
                {
                    case 0: return "№ п/п";
                    case 1: return "Вид работы";
                    case 2: return "Объем работы, ч";
                    case 3: return ""; // Пустота между планируемым и фактическим объемом
                    case 4: return "Отклонение (+,-)";
                    default: return string.Empty;
                }
            }
            if (row == 1)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "";
                    case 2: return "планируемый";
                    case 3: return "фактический";
                    case 4: return "";
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        public string GetMainBodyText(int row, int col)
        {
            if (row == 0)
            {
                switch (col)
                {
                    case 0: return "1";
                    case 1: return "Учебная работа, всего за учебный год";
                    default: return string.Empty;
                }
            }
            if (row == 1)
            {
                switch (col)
                {
                    case 1: return "в том числе";
                    default: return string.Empty;
                }
            }
            if (row == 2)
            {
                switch (col)
                {
                    case 2: return "1 семестр";
                    default: return string.Empty;
                }
            }
            if (row == 3)
            {
                switch (col)
                {
                    case 1: return "- сентябрь";
                    default: return string.Empty;
                }
            }
            if (row == 4)
            {
                switch (col)
                {
                    case 1: return "- октябрь";
                    default: return string.Empty;
                }
            }
            if (row == 5)
            {
                switch (col)
                {
                    case 1: return "- ноябрь";
                    default: return string.Empty;
                }
            }
            if (row == 6)
            {
                switch (col)
                {
                    case 1: return "- декабрь";
                    default: return string.Empty;
                }
            }
            if (row == 7)
            {
                switch (col)
                {
                    case 1: return "- январь";
                    default: return string.Empty;
              
                }
            }
            if (row == 8)
            {
                switch (col)
                {
                    case 1: return "- анкетирование студентов о качестве обучения";
                    default: return string.Empty;
                }
            }
            if (row == 9)
            {
                switch (col)
                {
                    case 1: return "Итого 1 семестр";
                    default: return string.Empty;
                }
            }
            if (row == 10)
            {
                switch (col)
                {
                    case 2: return "2 семестр";
                    default: return string.Empty;
                }
            }
            if (row == 11)
            {
                switch (col)
                {
                    case 1: return "- февраль";
                    default: return string.Empty;
                }
            }
            if (row == 11)
            {
                switch (col)
                {
                    case 1: return "- февраль";
                    default: return string.Empty;
                }
            }
            if (row == 12)
            {
                switch (col)
                {
                    case 1: return "- март";
                    default: return string.Empty;
                }
            }
            if (row == 13)
            {
                switch (col)
                {
                    case 1: return "- апрель";
                    default: return string.Empty;
                }
            }
            if (row == 14)
            {
                switch (col)
                {
                    case 1: return "- май";
                    default: return string.Empty;
                }
            }
            if (row == 15)
            {
                switch (col)
                {
                    case 1: return "- июнь";
                    default: return string.Empty;
                }
            }
            if (row == 16)
            {
                switch (col)
                {
                    case 1: return "- анкетирование студентов о качестве обучения";
                    default: return string.Empty;
                }
            }
            if (row == 17)
            {
                switch (col)
                {
                    case 1: return "Итого 2 семестр";
                    default: return string.Empty;
                }
            }
            if (row == 18)
            {
                switch (col)
                {
                    case 0: return "2";
                    case 1: return "Учебно-методическая работа";
                    default: return string.Empty;
                }
            }
            if (row == 19)
            {
                switch (col)
                {
                    case 0: return "3";
                    case 1: return "Организационно-методическая работа";
                    default: return string.Empty;
                }
            }
            if (row == 20)
            {
                switch (col)
                {
                    case 0: return "4";
                    case 1: return "Научно-исследовательская и инновационная работа";
                    default: return string.Empty;
                }
            }
            if (row == 21)
            {
                switch (col)
                {
                    case 0: return "5";
                    case 1: return "Профориентационная и воспитательная работа";
                    default: return string.Empty;
                }
            }
            return string.Empty;
        }

        public static Table CreateTable(List<ProcessActivityType> procActs)
        {
            // Создаем таблицу
            Table table = new Table();
            
            // Добавляем заголовок таблицы
            for (int i = 0; i < 4; i++)
            {
                TableRow row = new TableRow();

                for (int j = 0; j < 8; j++)
                {
                    TableCell cell = new TableCell();

                    // Добавляем текст в ячейку
                    string cellText = GetHeaderTextMainBody(i, j);
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    paragraph.ParagraphProperties = new ParagraphProperties(
                    new SpacingBetweenLines() { Before = "0", After = "0" });
                    paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                    cell.Append(paragraph);

                    var props = new TableCellProperties();
                    props.Append(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
                    cell.Append(props);

                    if ((i == 0 && j == 1) || (i == 0 && j == 8) || (i==0 && j == 0)) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    else if (j == 0 || j == 1 || j == 8 ) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 0 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 6 }));
                    if (i == 1 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 3 }));
                    if (i == 1 && j == 5) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 3 }));

                    if (i == 2 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i == 3 && j == 2) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 2 && j == 5) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i == 3 && j == 5) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 2 && j == 3) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    if (i == 2 && j == 6) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    
                    if ((i == 0 && j > 2 && j < 8) || (i == 1 && j > 2 && j < 5) || (i == 1 && j > 5 && j < 8) || (i==2&&j==4)||(i==2&&j==7)) continue;
                    row.Append(cell);
                }

                table.Append(row);
            }

            

            // Добавление тела таблицы
            int activityNumber = 0;
            foreach(var procAct in procActs)
            {
                if (procAct.DatePlan == null) procAct.DatePlan = "";
                if (procAct.DateFact == null) procAct.DateFact = "";
                TableRow procRow = new TableRow();
                activityNumber++;
                for (int i = 0; i < 8; i++)
                {
                    TableCell cell = new TableCell();
                    string cellText = string.Empty;
                    if (i == 0) cellText = activityNumber.ToString();
                    if (i == 1) cellText = procAct.Name;
                    if (i == 2) cellText = procAct.DatePlan;
                    if (i == 3) cellText = procAct.HoursPlan.ToString();
                    if (i == 4) cellText = procAct.UnitPlan.ToString();
                    if (i == 5) cellText = procAct.DateFact;
                    if (i == 6) cellText = procAct.HoursFact.ToString();
                    if (i == 7) cellText = procAct.UnitFact.ToString();
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    paragraph.ParagraphProperties = new ParagraphProperties(
                    new SpacingBetweenLines() { Before = "0", After = "0" });
                    if(i != 1) paragraph.ParagraphProperties.Append(new Justification() { Val = JustificationValues.Center });
                    cell.Append(paragraph);

                    var props = new TableCellProperties();
                    props.Append(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
                    cell.Append(props);

                    procRow.Append(cell);
                }
                table.Append(procRow);
            }

            // Добавляем границы таблицы
            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                )
            );

            table.Append(tableProperties);
            return table;
        }

        

        static string GetHeaderTextMainBody(int row, int col)
        {
            // Возвращает текст для ячейки таблицы в зависимости от её положения
            if (row == 0)
            {
                switch (col)
                {
                    case 0: return "№ п/п";
                    case 1: return "Название вида работы";
                    case 2: return "Объем работы";
                    case 3: return ""; // Пустота между планируемым и фактическим объемом
                    case 4: return "";
                    case 5: return ""; // Дополнительная пустота, если нужно
                    case 6: return ""; // Дополнительная пустота, если нужно
                    case 7: return "";
                    default: return string.Empty;
                }
            }
            if(row == 1)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "";
                    case 2: return "планируемый";
                    case 3: return "";
                    case 4: return "";
                    case 5: return "фактический";
                    case 6: return "";
                    case 7: return "";
                    default: return string.Empty;
                }
            }
            if(row == 2)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "";
                    case 2: return "дата";
                    case 3: return "количество";
                    case 4: return "";
                    case 5: return "дата";
                    case 6: return "количество";
                    case 7: return "";
                    default: return string.Empty;
                }
            }
            if(row == 3)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "";
                    case 2: return "дата";
                    case 3: return "в час";
                    case 4: return "в ед.";
                    case 5: return "дата";
                    case 6: return "в час";
                    case 7: return "в ед.";
                    default: return string.Empty;
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
