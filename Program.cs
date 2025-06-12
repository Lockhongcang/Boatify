using Boatify.Models;
using Boatify.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson();

builder.Services.AddSession();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<BookingService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddScoped<OrderIntegrationService>();

builder.Services.AddDbContext<BoatifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BoatifyConnection")));

builder.Services.AddAuthentication("AuthCookie")
    .AddCookie("AuthCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
