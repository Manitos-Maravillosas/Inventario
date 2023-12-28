using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Controllers.Admin
{
    public class ClientController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CRUD_Cliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                    new SqlParameter("@idClient", DBNull.Value),
                    new SqlParameter("@name", DBNull.Value),
                    new SqlParameter("@lastName1", DBNull.Value),
                    new SqlParameter("@lastName2", DBNull.Value),
                    new SqlParameter("@email", DBNull.Value),
                    new SqlParameter("@phoneNumber", DBNull.Value),
                    new SqlParameter("@operation", 2)
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Client client = new Client
                            {
                                Id = dataReader["idClient"].ToString(),
                                Name = dataReader["name"].ToString(),
                                LastName1 = dataReader["lastName1"].ToString(),
                                LastName2 = dataReader["lastName2"].ToString(),
                                Email = dataReader["email"].ToString(),
                                PhoneNumber = dataReader["phoneNumber"].ToString() // Agrega esto si la columna existe
                            };
                            clients.Add(client);
                        }
                    }
                }
            }

            return clients;
        }
        // GET: ClientController
        public ActionResult Index() // Nombre de la vista
        {
            var clients = GetClients();
            return View("~/Views/Admin/Client/Index.cshtml", clients);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
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

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientController/Edit/5
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

        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientController/Delete/5
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
