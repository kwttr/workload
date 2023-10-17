using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workload_Models;

namespace workload_DataAccess.Repository.IRepository
{
    public interface IProcessActivityTypeRepository : IRepository<ProcessActivityType>
    {
        void Update(ProcessActivityType obj);
    }
}
