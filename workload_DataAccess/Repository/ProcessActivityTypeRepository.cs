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
    public class ProcessActivityTypeRepository : Repository<ProcessActivityType>, IProcessActivityTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ProcessActivityTypeRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(ProcessActivityType obj)
        {
            _db.ProcessActivityType.Update(obj);
        }
    }
}
