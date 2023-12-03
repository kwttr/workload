using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workload.Services
{
    public interface IUsers
    {
        public bool LockUser(string email, DateTime? endDate);
        public bool UnlockUser(string email);
    }
    public class Users : IUsers
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DateTime EndDate;
        public Users(UserManager<IdentityUser> userMgr)
        {
            EndDate = new DateTime(2222, 06, 06);
            _userManager = userMgr;
        }
        public bool LockUser(string email, DateTime? endDate)
        {
            if (endDate == null)
                endDate = EndDate;
            var userTask = _userManager.FindByEmailAsync(email);
            userTask.Wait();
            var user = userTask.Result;
            var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
            lockUserTask.Wait();
            var lockDateTask = _userManager.SetLockoutEndDateAsync(user, endDate);
            lockDateTask.Wait();
            return lockDateTask.Result.Succeeded && lockUserTask.Result.Succeeded;
        }
        public bool UnlockUser(string email)
        {
            var userTask = _userManager.FindByEmailAsync(email);
            userTask.Wait();
            var user = userTask.Result;
            var lockDisabledTask = _userManager.SetLockoutEnabledAsync(user, false);
            lockDisabledTask.Wait();
            var setLockoutEndDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
            setLockoutEndDateTask.Wait();
            return setLockoutEndDateTask.Result.Succeeded && lockDisabledTask.Result.Succeeded;
        }
    }
}
