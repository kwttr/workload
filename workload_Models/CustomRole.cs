using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
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

    public class CustomUserManager<TUser> : UserManager<TUser> where TUser : class
    {
        private readonly RoleManager<CustomRole> _roleManager;

        public CustomUserManager(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger,
            RoleManager<CustomRole> roleManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleManager = roleManager;
        }
    }

    public class MyRoleValidator : RoleValidator<CustomRole>
    {
        public override Task<IdentityResult> ValidateAsync(RoleManager<CustomRole> manager, CustomRole role)
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
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
