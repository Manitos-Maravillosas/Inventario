using Azure.Core;
using Microsoft.EntityFrameworkCore;

using Sistema_Inventario_Manitos_Maravillosas.Models.Inventory;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
namespace Sistema_Inventario_Manitos_Maravillosas.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
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
        public DbSet<Sistema_Inventario_Manitos_Maravillosas.Models.Inventory.Product>? Product { get; set; }
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


    }

    
}
