using Microsoft.EntityFrameworkCore;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
namespace Sistema_Inventario_Manitos_Maravillosas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        public DbSet<Product>? Product { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models.Client>? Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models.Employee>? Employee { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models.ProductCategory>? ProductCategories { get; set; }

        //Admin
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models.TypePayment>? TypePayment { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models.BankAccount>? BankAccount { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        //public DbSet<Client> Client { get; set; }
        //login


        //Facturation


        //Inventory

        //Admin
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models.TypeDelivery>? TypeDelivery { get; set; }


    }


}
