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
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext _db;
        public TeacherRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.DegreeName)
            {
                return _db.Degree.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.PositionName)
            {
                return _db.Position.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if(obj == WC.DepartmentName)
            {
                return _db.Department.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Teacher obj)
        {
            _db.Teachers.Update(obj);
        }
    }
}
