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
        public ReportRepository(ApplicationDbContext db): base(db)
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
            // Создание новой таблицы
            Table table = new Table();

            TableRow headerRow = new TableRow();
            headerRow.Append(CreateCell("Название вида работы", 4, 1));
            headerRow.Append(CreateCell("Объем работы", 1, 6));
            headerRow.Append(CreateCell("Норма часов", 4, 1));
            table.Append(headerRow);

            TableRow subHeaderRow1 = new TableRow();
            subHeaderRow1.Append(CreateCell("планируемый", 1, 3));
            subHeaderRow1.Append(CreateCell("фактический", 1, 3));
            table.Append(subHeaderRow1);

            TableRow subHeaderRow2 = new TableRow();
            subHeaderRow2.Append(CreateCell("дата", 2, 1));
            subHeaderRow2.Append(CreateCell("количество", 1, 2));
            subHeaderRow2.Append(CreateCell("дата", 2, 1));
            subHeaderRow2.Append(CreateCell("количество", 1, 2));
            table.Append(subHeaderRow2);

            TableRow subHeaderRow3 = new TableRow();
            subHeaderRow3.Append(CreateCell("в час", 1, 1));
            subHeaderRow3.Append(CreateCell("в ед.", 1, 1));
            subHeaderRow3.Append(CreateCell("в час", 1, 1));
            subHeaderRow3.Append(CreateCell("в ед.", 1, 1));
            table.Append(subHeaderRow3);

            // Создание строки данных
            TableRow dataRow = new TableRow();
            dataRow.Append(CreateCell("Данные1"));
            dataRow.Append(CreateCell("Данные2"));
            dataRow.Append(CreateCell("Данные3"));
            dataRow.Append(CreateCell("Данные4"));
            dataRow.Append(CreateCell("Данные5"));
            dataRow.Append(CreateCell("Данные6"));
            dataRow.Append(CreateCell("Данные7"));
            dataRow.Append(CreateCell("Данные8"));
            dataRow.Append(CreateCell("Данные9"));
            table.Append(dataRow);

            return table;
        }

        public static TableCell CreateCell(string text, int rowSpan = 1, int colSpan = 1)
        {
            TableCell cell = new TableCell();
            Paragraph paragraph = new Paragraph();
            Run run = new Run();
            run.AppendChild(new Text(text));
            paragraph.Append(run);
            cell.Append(paragraph);
            if (rowSpan > 1)
            {
                if (cell.TableCellProperties == null)
                {
                    cell.TableCellProperties = new TableCellProperties();
                }
                cell.TableCellProperties.AppendChild(new VerticalMerge() { Val = MergedCellValues.Restart });
                cell.TableCellProperties.AppendChild(new VerticalMerge() { Val = MergedCellValues.Continue });
            }
            if (colSpan > 1)
            {
                if (cell.TableCellProperties == null)
                {
                    cell.TableCellProperties = new TableCellProperties();
                }
                cell.TableCellProperties.AppendChild(new GridSpan() { Val = colSpan });
            }
            return cell;
        }
        #endregion
    }
}
