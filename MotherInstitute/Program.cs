using Microsoft.EntityFrameworkCore;
using MotherInstitute.Models;
using MotherInstitute.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ErrorLogger>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Home/Error");

app.UseSession();

app.UseAuthorization();


// ==========================================
// ADMIN HOME ROUTE
// ==========================================

app.MapControllerRoute(
    name: "AdminHome",
    pattern: "Admin/Home",
    defaults: new
    {
        controller = "Admin",
        action = "Home"
    });


// ==========================================
// EXECUTIVE HOME ROUTE
// ==========================================

app.MapControllerRoute(
    name: "ExecutiveHome",
    pattern: "Executive/Home",
    defaults: new
    {
        controller = "Executive",
        action = "Home"
    });


// ==========================================
// MARKETING HOME ROUTE
// ==========================================

app.MapControllerRoute(
    name: "MarketingHome",
    pattern: "Marketing/Home",
    defaults: new
    {
        controller = "Marketing",
        action = "Home"
    });


// ==========================================
// EXECUTIVE MASTER ROUTES
// ==========================================

app.MapControllerRoute(
    name: "ExecutiveAcademicSession",
    pattern: "Executive/AcademicSession",
    defaults: new
    {
        controller = "AcademicSession",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveOrganization",
    pattern: "Executive/Organization",
    defaults: new
    {
        controller = "Organization",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveSubjects",
    pattern: "Executive/Subjects",
    defaults: new
    {
        controller = "Subjects",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveBedDetails",
    pattern: "Executive/BedDetails",
    defaults: new
    {
        controller = "BedDetails",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveCourse",
    pattern: "Executive/Course",
    defaults: new
    {
        controller = "Course",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveFees",
    pattern: "Executive/Fees",
    defaults: new
    {
        controller = "Fees",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveEizocMember",
    pattern: "Executive/EizocMember",
    defaults: new
    {
        controller = "EizocMember",
        action = "Index"
    });


// ==========================================
// EXECUTIVE REGISTRATION ROUTES
// ==========================================

app.MapControllerRoute(
    name: "ExecutiveRegistration",
    pattern: "Executive/Registration",
    defaults: new
    {
        controller = "Registration",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveViewStudentsRegistration",
    pattern: "Executive/ViewStudentsRegistration",
    defaults: new
    {
        controller = "ViewStudentsRegistration",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveStudentStatus",
    pattern: "Executive/StudentStatus",
    defaults: new
    {
        controller = "StudentStatus",
        action = "Index"
    });


// ==========================================
// EXECUTIVE ACCOUNTS ROUTES
// ==========================================

app.MapControllerRoute(
    name: "ExecutiveStudentExpenses",
    pattern: "Executive/StudentExpenses",
    defaults: new
    {
        controller = "StudentExpenses",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveStudentPayments",
    pattern: "Executive/StudentPayments",
    defaults: new
    {
        controller = "StudentPayments",
        action = "Index"
    });


// ==========================================
// EXECUTIVE MARKETING ROUTES
// ==========================================

app.MapControllerRoute(
    name: "ExecutiveSchoolMaster",
    pattern: "Executive/SchoolMaster",
    defaults: new
    {
        controller = "SchoolMaster",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveMarketingAgent",
    pattern: "Executive/MarketingAgent",
    defaults: new
    {
        controller = "MarketingAgent",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveMarketingStudent",
    pattern: "Executive/MarketingStudent",
    defaults: new
    {
        controller = "MarketingStudent",
        action = "Index"
    });

app.MapControllerRoute(
    name: "ExecutiveVisitStudent",
    pattern: "Executive/MarketingPanel/VisitStudent",
    defaults: new
    {
        controller = "MarketingPanel",
        action = "VisitStudent"
    });


// ==========================================
// ADMIN USER ROUTE
// ==========================================

app.MapControllerRoute(
    name: "AdminUser",
    pattern: "Admin/User",
    defaults: new
    {
        controller = "User",
        action = "Index"
    });


// ==========================================
// MARKETING ROUTES
// ==========================================

app.MapControllerRoute(
    name: "MarketingVisitStudent",
    pattern: "Marketing/MarketingPanel/VisitStudent",
    defaults: new
    {
        controller = "MarketingPanel",
        action = "VisitStudent"
    });

app.MapControllerRoute(
    name: "MarketingAgentWiseReport",
    pattern: "Marketing/MarketingPanel/AgentWiseReport",
    defaults: new
    {
        controller = "MarketingPanel",
        action = "AgentWiseReport"
    });


// ==========================================
// DEFAULT ROUTE
// ==========================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();