using DinkToPdf;
using DinkToPdf.Contracts;
using EmailService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Helper;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Identity.Data;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Data;
using Sistema_Inventario_Manitos_Maravillosas.Data.Services;
using Sistema_Inventario_Manitos_Maravillosas.Filters;
using SistemaInventario.Data;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Data.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//-------------------------------------SERVICES------------------------------------------------//
// Facturation Services
builder.Services.AddScoped<IProductServiceFacturation, ProductServiceFacturation>();

builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();


//Admin Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<ITypePaymentService, TypePaymentService>();
builder.Services.AddScoped<ICoinService, CoinService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IPurchaseProviderService, PurchaseProviderService>();
builder.Services.AddScoped<ITypeDeliveryService, TypeDeliveryService>();

//Inventory Services
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Reports Services
builder.Services.AddScoped<IReportsService, ReportsService>();

//Other Services
builder.Services.AddSingleton<IFileLogger, FileLogger>();
builder.Services.AddScoped<BillHandler>();

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
//-------------------------------------END SERVICES------------------------------------------------//

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});


// Add the DinkToPdf service
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionToDataBase")));

builder.Services.AddDbContext<InventarioDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionToDataBase")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InventarioDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient(); // Register HttpClient

// Add IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(); // Add session services
builder.Services.AddMemoryCache(); // Add memory cache services

builder.Services.Configure<SignInOptions>(options =>
{
    options.RequireConfirmedEmail = false;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var cultures = new[]
{
 new CultureInfo("en-US"),
 new CultureInfo("de"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = cultures,
    SupportedUICultures = cultures
});
app.UseStatusCodePagesWithReExecute("/Error/Index/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // Add this line to use session middleware

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "areas",
    pattern: "{area?}/{controller=Bill}/{action=Index}/{id?}");
});


app.MapRazorPages();

app.Run();
