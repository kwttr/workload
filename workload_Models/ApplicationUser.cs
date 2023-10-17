using Microsoft.AspNetCore.Identity;

namespace workload_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
