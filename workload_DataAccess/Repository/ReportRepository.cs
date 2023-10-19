using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
