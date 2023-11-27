namespace workload_Models.ViewModels
{
    public class HomeViewModel
    {
        public List<DepartmentHodWindow>? DepartmentsHod { get; set; }
        public List<DepartmentTeacherWindow>? DepartmentsTeacher { get; set; }
    }

    public class DepartmentHodWindow
    {
        public Department Department { get; set; }
        public int? WorkersCount { get; set; }
        public int? ReportsToApproveCount { get; set; }
    }

    public class DepartmentTeacherWindow
    {
        public Department Department { get; set; }
        public int? ReportsAssignedCount { get; set; }
    }
}
