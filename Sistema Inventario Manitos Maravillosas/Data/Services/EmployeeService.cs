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


        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(Employee newEmployee)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spEmployeeCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Configure parameters
                        command.Parameters.Add(new SqlParameter("@idEmployee", string.IsNullOrEmpty(newEmployee.IdEmployee) ? DBNull.Value : newEmployee.IdEmployee));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newEmployee.Name) ? DBNull.Value : newEmployee.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newEmployee.LastName1) ? DBNull.Value : newEmployee.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newEmployee.LastName2) ? DBNull.Value : newEmployee.LastName2));
                        command.Parameters.Add(new SqlParameter("@position", string.IsNullOrEmpty(newEmployee.Position) ? DBNull.Value : newEmployee.Position));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newEmployee.PhoneNumber) ? DBNull.Value : newEmployee.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idBusiness", newEmployee.IdBusiness != 0 ? (object)newEmployee.IdBusiness : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@BusinessName", string.IsNullOrEmpty(newEmployee.BusinessName) ? DBNull.Value : newEmployee.BusinessName));
                        command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(newEmployee.Email) ? DBNull.Value : newEmployee.Email));
                        command.Parameters.Add(new SqlParameter("@operation", 1));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Check if the error is a custom error thrown using RAISEERROR
                if (sqlEx.Number == 50000) // 50000 is the default error number for RAISEERROR
                {
                    // Handle custom error
                    result.Success = false;
                    result.Message = sqlEx.Message; // This will contain the custom message from RAISEERROR
                    return result;
                }
                else
                {
                    // You can use different strategies to relay this message back to the user.
                    // For example, you might throw a new exception with the user-friendly message,
                    // or you could return an error response that your frontend can use to display the alert.
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }


            }
            catch (Exception ex)
            {
                // Handle non-SQL exceptions here
                // Log the exception, and/or rethrow, or return a specific error message
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(Employee newEmployee)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spEmployeeCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Configure parameters
                        command.Parameters.Add(new SqlParameter("@idEmployee", string.IsNullOrEmpty(newEmployee.IdEmployee) ? DBNull.Value : newEmployee.IdEmployee));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newEmployee.Name) ? DBNull.Value : newEmployee.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newEmployee.LastName1) ? DBNull.Value : newEmployee.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newEmployee.LastName2) ? DBNull.Value : newEmployee.LastName2));
                        command.Parameters.Add(new SqlParameter("@position", string.IsNullOrEmpty(newEmployee.Position) ? DBNull.Value : newEmployee.Position));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newEmployee.PhoneNumber) ? DBNull.Value : newEmployee.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idBusiness", newEmployee.IdBusiness != 0 ? (object)newEmployee.IdBusiness : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@BusinessName", string.IsNullOrEmpty(newEmployee.BusinessName) ? DBNull.Value : newEmployee.BusinessName));
                        command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(newEmployee.Email) ? DBNull.Value : newEmployee.Email));
                        command.Parameters.Add(new SqlParameter("@operation", 3));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                string userMessage = "An error occurred while processing your request.";

                
                if (sqlEx.Number == 50000) // 50000 is the default error number for RAISEERROR
                {
                    // Handle custom error
                    result.Success = false;
                    result.Message = sqlEx.Message; // This will contain the custom message from RAISEERROR
                    return result;
                }
                else
                {
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }


            }
            catch (Exception ex)
            {                
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }

        //------------------------------------------------------------------------------------
        //                              GetById                                             
        //------------------------------------------------------------------------------------
        public Employee GetById(string id)
        {
            Employee employee = new Employee();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spEmployeeCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idEmployee", id),
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
                            
                            while (dataReader.Read())
                            {
                                 employee = new Employee
                                {
                                    IdEmployee = dataReader["idEmployee"].ToString(),
                                    Name = dataReader["name"].ToString(),
                                    LastName1 = dataReader["lastName1"].ToString(),
                                    LastName2 = dataReader["lastName2"].ToString(),
                                    Position = dataReader["position"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    BusinessName = dataReader["businessName"].ToString(),
                                    Email = dataReader["email"].ToString(),
                                };
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return employee;
        }

        //------------------------------------------------------------------------------------
        //                              Delete                                             
        //------------------------------------------------------------------------------------
        public OperationResult Delete(string id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spEmployeeCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idEmployee", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@lastName1", DBNull.Value),
                            new SqlParameter("@lastName2", DBNull.Value),
                            new SqlParameter("@position", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@idBusiness", DBNull.Value),
                            new SqlParameter("@BusinessName", DBNull.Value),
                            new SqlParameter("@email", DBNull.Value),
                            new SqlParameter("@operation", '4')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();
                        command.ExecuteReader();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {

                if (sqlEx.Number == 50000) // 50000 is the default error number for RAISEERROR
                {
                    // Handle custom error
                    result.Success = false;
                    result.Message = sqlEx.Message; // This will contain the custom message from RAISEERROR
                    return result;
                }
                else
                {
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }
    }
}
