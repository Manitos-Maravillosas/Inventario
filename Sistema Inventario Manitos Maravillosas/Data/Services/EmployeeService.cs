using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public EmployeeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null; // Declarar la conexión fuera del bloque try-catch

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spEmployeeCRUD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@idEmployee", DBNull.Value),
                        new SqlParameter("@name", DBNull.Value),
                        new SqlParameter("@lastName1", DBNull.Value),
                        new SqlParameter("@lastName2", DBNull.Value),
                        new SqlParameter("@position", DBNull.Value),
                        new SqlParameter("@phoneNumber", DBNull.Value),
                        new SqlParameter("@idBusiness", DBNull.Value),
                        new SqlParameter("@email", DBNull.Value),
                        new SqlParameter("@operation", '2')
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            // Handle the case when no data is returned
                            // Puedes lanzar una excepción personalizada o registrar el error según tus necesidades
                            throw new Exception("No se encontraron registros.");
                        }

                        while (dataReader.Read())
                        {
                            Employee employee = new Employee
                            {
                                IdEmployee = dataReader["idEmployee"].ToString(),
                                Name = dataReader["name"].ToString(),
                                LastName1 = dataReader["lastName1"].ToString(),
                                LastName2 = dataReader["lastName2"].ToString(),
                                Position = dataReader["position"].ToString(),
                                PhoneNumber = dataReader["phoneNumber"].ToString(),
                                BusinessName = dataReader["BusinessName"].ToString(),
                                Email = dataReader["email"].ToString(),
                            };
                            employees.Add(employee);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Puedes lanzar una excepción personalizada o registrar el error según tus necesidades
                throw new Exception("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión en caso de excepción o no
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return employees;
        }

        //------------------------------------------------------------------------------------
        //                              GetBusinessNames                                             
        //------------------------------------------------------------------------------------
        public List<string> GetBusinessNames()
        {
            List<string> businessNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null; // Declarar la conexión fuera del bloque try-catch

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getBusinessNamesCommand = new SqlCommand("spGetBusinessNames", connection))
                {
                    getBusinessNamesCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader businessDataReader = getBusinessNamesCommand.ExecuteReader())
                    {
                        while (businessDataReader.Read())
                        {
                            string businessName = businessDataReader["name"].ToString();
                            businessNames.Add(businessName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Puedes lanzar una excepción personalizada o registrar el error según tus necesidades
                throw new Exception("Error al obtener nombres de negocios.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión en caso de excepción o no
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return businessNames;
        }

        //------------------------------------------------------------------------------------
        //                              GetUserEmails                                             
        //------------------------------------------------------------------------------------
        public List<string> GetUserEmails()
        {
            List<string> userEmails = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null; // Declarar la conexión fuera del bloque try-catch

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getUserEmailsCommand = new SqlCommand("spGetUserEmail", connection))
                {
                    getUserEmailsCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader userEmailDataReader = getUserEmailsCommand.ExecuteReader())
                    {
                        while (userEmailDataReader.Read())
                        {
                            string userEmail = userEmailDataReader["email"].ToString();
                            userEmails.Add(userEmail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Puedes lanzar una excepción personalizada o registrar el error según tus necesidades
                throw new Exception("Error al obtener correos electrónicos de usuarios.", ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión en caso de excepción o no
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return userEmails;
        }





    }
}
