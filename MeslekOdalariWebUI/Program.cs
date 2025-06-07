using FluentValidation;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.Entity.Entities;
using MeslekOdalariWebUI.Extensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Reflection;
using MeslekOdalari.Business.Services;
using Microsoft.AspNetCore.Identity;
using MeslekOdalari.Entity.Entities.Enums;
using MeslekOdalariWebUI.Models.Services;
using MeslekOdalariWebUI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServiceExtensions();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var mongoDatabase = new MongoClient(builder.Configuration.GetConnectionString("MongoConnection")).GetDatabase(builder.Configuration.GetSection("DatabaseName").Value);
builder.Services.AddDbContext<MeslekOdalariContext>(option =>
{
    option.UseMongoDB(mongoDatabase.Client, mongoDatabase.DatabaseNamespace.DatabaseName);
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<MeslekOdalariContext>();
builder.Services.AddScoped<IRoleSeedService, RoleSeedService>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//mail için
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();



// RSS servisleri ekle
builder.Services.AddHttpClient<MeslekOdalariWebUI.Services.IRssService, MeslekOdalariWebUI.Services.RssService>();
builder.Services.AddScoped<MeslekOdalariWebUI.Services.IRssService, MeslekOdalariWebUI.Services.RssService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Rolleri oluþtur
        var roleSeedService = services.GetRequiredService<IRoleSeedService>();
        await roleSeedService.SeedRolesAsync();

        // Admin kullanýcýsýný oluþtur
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        await CreateAdminUser(userManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed iþlemleri sýrasýnda hata: {ex.Message}");
    }
}

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

app.Run();

// Admin kullanýcýsý oluþturma metodu
static async Task CreateAdminUser(UserManager<AppUser> userManager)
{
    var adminTC = "11111111111"; // Admin TC kimlik no
    var adminUser = await userManager.Users.FirstOrDefaultAsync(u => u.TC == adminTC);

    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            TC = adminTC,
            NameSurName = "Sistem Yöneticisi",
            Email = "admin@meslekodasi.com",
            UserName = "admin",
            UserRole = UserRoles.Admin,
            IsApproved = true,
            RegistrationDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            Console.WriteLine("? Admin kullanýcýsý baþarýyla oluþturuldu.");
            Console.WriteLine($"   TC: {adminUser.TC}");
            Console.WriteLine($"   Email: {adminUser.Email}");
            Console.WriteLine($"   Kullanýcý Adý: {adminUser.UserName}");
            Console.WriteLine($"   Þifre: Admin123!");
        }
        else
        {
            Console.WriteLine("? Admin kullanýcýsý oluþturulamadý:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"   - {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("?? Admin kullanýcýsý zaten mevcut.");
    }
}