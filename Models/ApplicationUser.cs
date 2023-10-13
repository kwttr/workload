using Microsoft.AspNetCore.Identity;

namespace workload.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
