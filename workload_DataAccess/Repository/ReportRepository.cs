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
        private readonly WordExporter _wordExporter;

        public ReportRepository(ApplicationDbContext db, ICategoryRepository catRepo) : base(db)
        {
            _db = db;
            _catRepo = catRepo;
            _wordExporter = new WordExporter(catRepo);
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

        public MemoryStream Export(Report obj)
        {
            return _wordExporter.Export(obj);
        }
    }
}
