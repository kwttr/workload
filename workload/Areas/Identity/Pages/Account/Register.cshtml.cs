// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using workload_Data;
using workload_DataAccess.ClaimTypes;
using workload_DataAccess.Repository.IRepository;
using workload_Models;
using workload_Utility;

namespace workload.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly ITeacherRepository _teachRepo;
        private readonly IDepartmentRepository _departmentRepo;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<CustomRole> roleManager,
            ITeacherRepository teachRepo,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _teachRepo = teachRepo;
            _departmentRepo = departmentRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> Roles;

        public IEnumerable<SelectListItem> Degrees;

        public IEnumerable<SelectListItem> Positions;
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string SelectedRole { get; set; }
            public string FullName { get; set; }
            public int DegreeId { get; set; }
            public int PositionId { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if(!await _roleManager.RoleExistsAsync(WC.AdminRole))
            {
                _roleManager.RoleValidators.Clear();
                _roleManager.RoleValidators.Add(new MyRoleValidator());
                List<Department> depList = _departmentRepo.GetAll().ToList();
                await _roleManager.CreateAsync(new CustomRole
                {
                    Name = WC.AdminRole,
                    DepartmentId = 1
                });
                foreach(var obj in  depList)
                {
                    var result = await _roleManager.CreateAsync(new CustomRole
                    {
                        Name = WC.HeadOfDepartmentRole+obj.Id.ToString(),
                        DepartmentId = obj.Id
                    });
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to create HeadOfDepartmentRole");
                    }
                    result = await _roleManager.CreateAsync(new CustomRole
                    {
                        Name = WC.TeacherRole+obj.Id.ToString(),
                        DepartmentId = obj.Id
                    });
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to create TeacherRole");
                    }
                }
            }
            Degrees = _teachRepo.GetAllDropdownList(WC.DegreeName);
            Positions = _teachRepo.GetAllDropdownList(WC.PositionName);

            if (User.IsInRole(WC.HeadOfDepartmentRole))
            {
                Roles = _roleManager.Roles.Where(x => x.NormalizedName == "TEACHER").Select(i => new SelectListItem
                {
                    Text = i.Name + " " + i.Department.Name,
                    Value = i.Id.ToString()
                });
            }
            else
            {
                Roles = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name + " " + i.Department.Name,
                    Value = i.Id.ToString()
                });
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Teacher { UserName = Input.Email, Email = Input.Email, FullName = Input.FullName, PositionId = Input.PositionId, DegreeId = Input.DegreeId };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var role = _roleManager.Roles.FirstOrDefault(r=>r.Id==Input.SelectedRole);
                    if (_userManager.Users.Count() > 1)
                    {
                        if (User.IsInRole(WC.AdminRole))
                        {
                            //Администратор создает нового пользователя = создается админ
                            if (role.Name.Contains(WC.AdminRole)) await _userManager.AddToRoleAsync(user, role.Name);
                        }
                        if (role.Name.Contains(WC.HeadOfDepartmentRole))
                        {
                            //нужно найти список всех юзеров в этой кафедре, дальше проверить наличие у юзеров роли зав.кафедры. Если нулл - регаем. Нет - ошибка.
                            var usersWithRoleHod = _userManager.GetUsersInRoleAsync(Input.SelectedRole).Result;
                            if (usersWithRoleHod.Count()>0)
                            {
                                await _userManager.DeleteAsync(user);
                                return Page();
                            }
                            await _userManager.AddToRoleAsync(user,role.Name);
                        }
                        if (User.IsInRole(WC.AdminRole) || User.IsInRole(WC.HeadOfDepartmentRole))
                        {
                            if (role.Name .Contains(WC.TeacherRole))
                            {
                                var resultrole = await _userManager.AddToRoleAsync(user, role.Name);
                                if (!resultrole.Succeeded)
                                {
                                    await _userManager.DeleteAsync(user);
                                    throw new InvalidOperationException("Failed to create TeacherRole");
                                }
                            }
                        }
                    }
                    else if (_userManager.Users.Count() <= 1)
                    {
                        var res = await _userManager.AddToRoleAsync(user,role.Name);
                        if(!res.Succeeded) { }
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var claim = new Claim("CustomClaimType", "DepartmentId");
                    await _userManager.AddClaimAsync(user, claim);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (!User.IsInRole(WC.AdminRole))
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
