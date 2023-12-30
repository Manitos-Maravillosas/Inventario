using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Models.Inventory;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Controllers.Facturation
{
    public class FacturationController : Controller
    {
        public IConfiguration Configuration { get; }
        public FacturationController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //Models for the view
        public Product Product { get; set; }


        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            string connectionString = Configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Client", connection);
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Client client = new Client();
                client.Id = Convert.ToString(dataReader["idClient"]);
                client.Name = Convert.ToString(dataReader["Name"]);
                client.LastName1 = Convert.ToString(dataReader["LastName1"]);
                client.LastName2 = Convert.ToString(dataReader["LastName2"]);
                client.Email = Convert.ToString(dataReader["Email"]);
                clients.Add(client);
            }

            connection.Close();

            return clients;
        }

        //get products
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string connectionString = Configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Product", connection);
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Product product = new Product();
                product.IdProduct = Convert.ToString(dataReader["IdProduct"]);
                product.ProductName = Convert.ToString(dataReader["Product"]);
                product.Stock = Convert.ToInt32(dataReader["Stock"]);
                product.Cost = Convert.ToInt32(dataReader["Cost"]);
                product.Description = Convert.ToString(dataReader["Description"]);
                product.Status = Convert.ToBoolean(dataReader["Status"]);
                product.IdBusiness = Convert.ToInt32(dataReader["IdBusiness"]);
                product.IdProductCategory = Convert.ToInt32(dataReader["IdProductCategory"]);
                products.Add(product);
            }

            connection.Close();

            return products;
        }

        // GET: FacturationController
        public ActionResult Index()
        {

            List<Product> products = GetProducts();

            ViewBag.Title = "Facturaasdfasdftion";
            return View(products);
        }

        // GET: FacturationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FacturationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FacturationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacturationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacturationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacturationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FacturationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
