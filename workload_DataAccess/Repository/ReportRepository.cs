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
        public ReportRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
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

                    //Добавление текста в документ
                    Table table = CreateTable(obj);
                    body.Append(table);

                    mainPart.Document.Save();
                }

                return stream;
            }
        }

        public static Table CreateTable(Report obj)
        {


            // Создаем таблицу
            Table table = new Table();
            
            // Добавляем заголовок таблицы
            for (int i = 0; i < 4; i++)
            {
                TableRow row = new TableRow();

                for (int j = 0; j < 7; j++)
                {
                    TableCell cell = new TableCell();

                    // Добавляем текст в ячейку
                    string cellText = GetHeaderText(i, j);
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    cell.Append(paragraph);

                    if ((i == 0 && j == 0) || i == 0 && j == 7) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    else if (j == 0 || j == 7) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 0 && j == 1) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 6 }));
                    if (i == 1 && j == 1) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 3 }));
                    if (i == 1 && j == 4) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 3 }));

                    if (i == 2 && j == 1) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i == 3 && j == 1) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 2 && j == 4) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    if (i == 3 && j == 4) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));

                    if (i == 2 && j == 2) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    if (i == 2 && j == 5) cell.AppendChild(new TableCellProperties(new GridSpan() { Val = 2 }));
                    
                    if ((i == 0 && j > 1 && j < 7) || (i == 1 && j > 1 && j < 4) || (i == 1 && j > 4 && j < 7) || (i==2&&j==3)||(i==2&&j==6)) continue;
                    row.Append(cell);
                }
                table.Append(row);
            }

            // Добавление тела таблицы
            foreach(var procAct in obj.ProcessActivities)
            {
                if (procAct.DatePlan == null) procAct.DatePlan = "";
                if (procAct.DateFact == null) procAct.DateFact = "";
                TableRow procRow = new TableRow();
                for(int i = 0; i < 7; i++)
                {
                    TableCell cell = new TableCell();
                    string cellText = string.Empty;
                    if (i == 0) cellText = procAct.Name;
                    if (i == 1) cellText = procAct.DatePlan;
                    if (i == 2) cellText = procAct.HoursPlan.ToString();
                    if (i == 3) cellText = procAct.UnitPlan.ToString();
                    if (i == 4) cellText = procAct.DateFact;
                    if (i == 5) cellText = procAct.HoursFact.ToString();
                    if (i == 6) cellText = procAct.UnitFact.ToString();
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    cell.Append(paragraph);
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
                    case 0: return "Название вида работы";
                    case 1: return "Объем работы";
                    case 2: return ""; // Пустота между планируемым и фактическим объемом
                    case 3: return "";
                    case 4: return ""; // Дополнительная пустота, если нужно
                    case 5: return ""; // Дополнительная пустота, если нужно
                    case 6: return "";
                    default: return string.Empty;
                }
            }
            if(row == 1)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "Планируемый";
                    case 2: return "";
                    case 3: return "";
                    case 4: return "фактический";
                    case 5: return "";
                    case 6: return "";
                    default: return string.Empty;
                }
            }
            if(row == 2)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "дата";
                    case 2: return "количество";
                    case 3: return "";
                    case 4: return "дата";
                    case 5: return "количество";
                    case 6: return "";
                    default: return string.Empty;
                }
            }
            if(row == 3)
            {
                switch (col)
                {
                    case 0: return "";
                    case 1: return "дата";
                    case 2: return "в час";
                    case 3: return "в ед.";
                    case 4: return "дата";
                    case 5: return "в час";
                    case 6: return "в ед.";
                    default: return string.Empty;
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
