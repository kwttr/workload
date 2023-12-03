using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using workload.Services;
using workload_Data;
using workload_DataAccess.Repository;
using workload_DataAccess.Repository.IRepository;
using workload_Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite("Filename = Database.db"));

builder.Services.AddIdentity<IdentityUser, CustomRole>()
    .AddRoleManager<CustomRoleManager>()
    .AddUserManager<CustomUserManager<IdentityUser>>()
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
        new TokenProviderDescriptor(
            typeof(CustomEmailConfirmationTokenProvider<IdentityUser>)));
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
});

builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();
builder.Services.AddScoped<RoleManager<CustomRole>, CustomRoleManager>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IProcessActivityTypeRepository, ProcessActivityTypeRepository>();
builder.Services.AddScoped<ITeacherDepartmentRepository, TeacherDepartmentRepository>();
builder.Services.AddScoped<IUsers, Users>();
builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 268435456; });

builder.Services.AddTransient<IEmailSender,EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
app.Run();
