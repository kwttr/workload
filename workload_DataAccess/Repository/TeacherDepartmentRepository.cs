using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workload_Data;
using workload_DataAccess.Repository.IRepository;
using workload_Models;

namespace workload_DataAccess.Repository
{
    public class TeacherDepartmentRepository : Repository<TeacherDepartment>, ITeacherDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public TeacherDepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
