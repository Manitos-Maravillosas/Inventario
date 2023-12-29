using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

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

        private void AddClient(Client client)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CRUD_Cliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Configura los parámetros aquí basándote en el objeto 'client'
                    // Verificar si cada propiedad es nula y asignar DBNull.Value en ese caso
                    command.Parameters.Add(new SqlParameter("@idClient", string.IsNullOrEmpty(client.Id) ? (object)DBNull.Value : client.Id));
                    command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(client.Name) ? (object)DBNull.Value : client.Name));
                    command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(client.LastName1) ? (object)DBNull.Value : client.LastName1));
                    command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(client.LastName2) ? (object)DBNull.Value : client.LastName2));
                    command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(client.Email) ? (object)DBNull.Value : client.Email));
                    command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(client.PhoneNumber) ? (object)DBNull.Value : client.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@operation", 1)); 

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ModifyClient(Client client)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CRUD_Cliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Configura los parámetros aquí basándote en el objeto 'client'
                    // Verificar si cada propiedad es nula y asignar DBNull.Value en ese caso
                    command.Parameters.Add(new SqlParameter("@idClient", string.IsNullOrEmpty(client.Id) ? (object)DBNull.Value : client.Id));
                    command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(client.Name) ? (object)DBNull.Value : client.Name));
                    command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(client.LastName1) ? (object)DBNull.Value : client.LastName1));
                    command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(client.LastName2) ? (object)DBNull.Value : client.LastName2));
                    command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(client.Email) ? (object)DBNull.Value : client.Email));
                    command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(client.PhoneNumber) ? (object)DBNull.Value : client.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("@operation", 3));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteClient(int id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CRUD_Cliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@idClient", id));
                    command.Parameters.Add(new SqlParameter("@name", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@lastName1", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@lastName2", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@email", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@phoneNumber", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@operation", 4));

                    connection.Open();

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            // No se eliminó ningún registro, puedes registrar un mensaje de error si lo deseas
                            Console.WriteLine("No se eliminó ningún registro.");
                        }
                        else
                        {
                            // Éxito: el cliente se eliminó correctamente
                            Console.WriteLine("Cliente eliminado correctamente.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Error al eliminar el cliente, registra el error
                        Console.WriteLine("Error al eliminar el cliente: " + ex.Message);
                    }
                }
            }
        }



        // GET: ClientController
        public ActionResult Index() // Nombre de la vista
        {
            var clients = GetClients();
            return View("~/Views/Admin/Client/Index.cshtml", clients);
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View("~/Views/Admin/Client/Create.cshtml");
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                AddClient(client);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Volver a la vista con el modelo actual para mostrar errores de validación
                return View("~/Views/Admin/Client/Create.cshtml", client);
            }
        }


        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            Client client = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CRUD_Cliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@idClient", id));
                    command.Parameters.Add(new SqlParameter("@name", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@lastName1", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@lastName2", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@email", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@phoneNumber", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@operation", 2)); // Asumiendo 3 para leer

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            client = new Client
                            {
                                Id = dataReader["idClient"].ToString(),
                                Name = dataReader["name"].ToString(),
                                LastName1 = dataReader["lastName1"].ToString(),
                                LastName2 = dataReader["lastName2"].ToString(),
                                Email = dataReader["email"].ToString(),
                                PhoneNumber = dataReader["phoneNumber"].ToString()
                            };
                        }
                    }
                }
            }

            if (client == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Client/Edit.cshtml", client);
        }


        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                // Realiza la modificación solo si el modelo es válido
                ModifyClient(client);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Volver a la vista con el modelo actual y los errores de validación
                return View("~/Views/Admin/Client/Edit.cshtml", client);
            }
        }


        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            DeleteClient(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
