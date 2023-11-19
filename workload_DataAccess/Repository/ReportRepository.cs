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
            Table table = new Table();

            // Создаем заголовки столбцов
            TableRow headerRow = new TableRow();

            TableCell headerCell1 = CreateHeaderCell("Название вида работы", 4);
            headerRow.AppendChild(headerCell1);

            TableCell headerCell2 = CreateHeaderCell("Объем работы", 4);
            headerRow.AppendChild(headerCell2);

            TableCell headerCell3 = CreateHeaderCell("Норма часов", 4);
            headerRow.AppendChild(headerCell3);

            table.AppendChild(headerRow);

            // Создаем строки для планируемого объема работы
            TableRow plannedRow = new TableRow();

            TableCell plannedCell = CreateCell("Планируемый", 1, 3);
            plannedRow.AppendChild(plannedCell);

            TableCell plannedDateCell = CreateCell("Дата", 1, 1);
            plannedRow.AppendChild(plannedDateCell);

            TableCell plannedHourCell = CreateCell("В час", 1, 1);
            plannedRow.AppendChild(plannedHourCell);

            TableCell plannedUnitCell = CreateCell("В ед.", 1, 1);
            plannedRow.AppendChild(plannedUnitCell);

            table.AppendChild(plannedRow);

            // Создаем строки для фактического объема работы
            TableRow actualRow = new TableRow();

            TableCell actualCell = CreateCell("Фактический", 1, 3);
            actualRow.AppendChild(actualCell);

            TableCell actualDateCell = CreateCell("Дата", 1, 1);
            actualRow.AppendChild(actualDateCell);

            TableCell actualHourCell = CreateCell("В час", 1, 1);
            actualRow.AppendChild(actualHourCell);

            TableCell actualUnitCell = CreateCell("В ед.", 1, 1);
            actualRow.AppendChild(actualUnitCell);

            table.AppendChild(actualRow);

            return table;
        }

        private static TableCell CreateHeaderCell(string cellText, int colspan)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(new GridSpan() { Val = colspan }));
            cell.Append(new Paragraph(new Run(new Text(cellText))));
            return cell;
        }

        private static TableCell CreateCell(string cellText, int colspan, int rowspan)
        {
            TableCell cell = new TableCell();
            cell.Append(new TableCellProperties(new GridSpan() { Val = colspan }, new VerticalMerge() { Val = MergedCellValues.Restart }));
            cell.Append(new Paragraph(new Run(new Text(cellText))));

            return cell;
        }
        #endregion
    }
}
