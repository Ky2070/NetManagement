using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<QuanLyNetContext>(options => {
<<<<<<< HEAD
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuanNet")); //Can change connect-string
=======
    options.UseSqlServer(builder.Configuration.GetConnectionString("NetShop")); //Can change connect-string
>>>>>>> 1041b051ac78f5e435718f0f97c85a9fc825ce5e
});

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
