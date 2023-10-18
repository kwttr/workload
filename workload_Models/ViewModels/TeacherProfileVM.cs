using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workload_Models.ViewModels
{
    public class TeacherProfileVM
    {
        public Teacher teacher { get; set; }
        public List<Report>? reportList { get; set; }
    }
}
