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
                    Table table = CreateTable(mainPart);
                    body.Append(table);

                    mainPart.Document.Save();
                }

                return stream;
            }
        }

        public static Table CreateTable(MainDocumentPart mainPart)
        {
            // Создаем таблицу
            Table table = new Table();

            // Добавляем ячейки и информацию
            for (int i = 0; i < 4; i++)
            {
                TableRow row = new TableRow();

                for (int j = 0; j < 8; j++)
                {
                    TableCell cell = new TableCell();

                    // Добавляем текст в ячейку
                    string cellText = GetHeaderText(i, j);
                    Paragraph paragraph = new Paragraph(new Run(new Text(cellText)));
                    cell.Append(paragraph);

                    if(j!=0) cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                    else cell.AppendChild(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));
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

        static string GetHeaderText(int row, int col)
        {
            // Возвращает текст для ячейки таблицы в зависимости от её положения
            if (row == 0)
            {
                switch (col)
                {
                    case 0: return "Название вида работы";
                    case 1: return "Планируемый объем работы";
                    case 2: return ""; // Пустота между планируемым и фактическим объемом
                    case 3: return "Фактический объем работы";
                    case 4: return ""; // Дополнительная пустота, если нужно
                    case 5: return ""; // Дополнительная пустота, если нужно
                    case 6: return "";
                    case 7: return "Норма часов";
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
                    case 2: return "количество";
                    case 3: return "";
                    case 4: return "";
                    case 5: return "количество";
                    case 6: return "";
                    case 7: return "";
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
                    case 7: return "";
                    default: return string.Empty;
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
