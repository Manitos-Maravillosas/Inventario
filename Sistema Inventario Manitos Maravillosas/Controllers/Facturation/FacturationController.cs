using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // GET: FacturationController
        public ActionResult Index()
        {
            List<Client> clients = GetClients();
            ViewBag.Title = "Facturaasdfasdftion";
            return View(clients);
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
