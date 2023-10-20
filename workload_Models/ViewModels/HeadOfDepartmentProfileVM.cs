using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workload_Models.ViewModels
{
    public class HeadOfDepartmentProfileVM
    {
        public Teacher Teacher { get; set; }
        public IEnumerable<Teacher>? ListTeacher { get; set; }
    }
}
