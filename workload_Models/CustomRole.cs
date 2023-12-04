using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string newPassword)
        {
            ThrowIfDisposed();
            var passwordStore = GetPasswordStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await UpdatePasswordHash(user, newPassword, true);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateUserAsync(user);
        }

        private IUserPasswordStore<TUser> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException();
            }
            return cast;
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
