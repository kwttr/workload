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
                    string cellText = GetHeaderText(i, j);
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

        static string GetHeaderText(int row, int col)
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
