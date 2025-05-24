using FluentValidation;
using MeslekOdalari.DataAccess.Context;
using MeslekOdalari.Entity.Entities;
using MeslekOdalariWebUI.Extensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Reflection;

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

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
