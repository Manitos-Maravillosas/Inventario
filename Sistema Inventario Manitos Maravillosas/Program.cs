using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Identity.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data;
using SistemaInventario.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionToDataBase")));

builder.Services.AddDbContext<InventarioDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionToDataBase")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<InventarioDbContext>();

builder.Services.AddHttpClient(); // Register HttpClient

builder.Services.AddSession(); // Add session services
builder.Services.AddMemoryCache(); // Add memory cache services

builder.Services.Configure<SignInOptions>(options =>
{
    options.RequireConfirmedEmail = false;
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
app.UseAuthentication(); ;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});


app.MapRazorPages();

app.Run();
