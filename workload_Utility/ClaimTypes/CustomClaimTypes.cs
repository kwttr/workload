using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace workload_Utility.ClaimTypes
{
    public class CustomClaimValue
    {
        public string RoleAccess { get; set; }
        public string DepartmentId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is CustomClaimValue other)
            {
                return this.RoleAccess == other.RoleAccess && this.DepartmentId == other.DepartmentId;
            }
            return false;
        }
    }
    public class CustomClaimType
    {
        public static string UserRoleDep = "UserRoleDep";
    }
}
