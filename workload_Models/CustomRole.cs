using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;


namespace workload_Models
{
    public class CustomRole : IdentityRole
    {
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public CustomRole() { }
    }

    public class CustomRoleManager : RoleManager<CustomRole>
    {
        public CustomRoleManager(
            IRoleStore<CustomRole> store,
            IEnumerable<IRoleValidator<CustomRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<CustomRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }

    public class MyRoleValidator : RoleValidator<CustomRole>
    {
        public override async Task<IdentityResult> ValidateAsync(RoleManager<CustomRole> manager, CustomRole role)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            var errors = new List<IdentityError>();
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }
    }
}
