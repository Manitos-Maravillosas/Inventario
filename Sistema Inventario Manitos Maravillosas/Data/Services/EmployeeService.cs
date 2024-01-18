using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee GetById(string id);
        Employee GetEmployeeByEmail(string email);
        List<string> GetRoleNames();
        public string GetUserId(string employeeId);
        List<string> GetUserEmails();
        OperationResult Add(Employee newEmployee);
        OperationResult Update(Employee newEmployee);
        OperationResult Delete(string id);
    }
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
            SqlConnection connection = null;

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
                employees.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return employees;
        }

        //------------------------------------------------------------------------------------
        //                              GetRoleNames                                             
        //------------------------------------------------------------------------------------
        public List<string> GetRoleNames()
        {
            List<string> roleNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getRolNamesCommand = new SqlCommand("spGetRoleNames", connection))
                {
                    getRolNamesCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader rolDataReader = getRolNamesCommand.ExecuteReader())
                    {
                        while (rolDataReader.Read())
                        {
                            string rolName = rolDataReader["Name"].ToString();
                            roleNames.Add(rolName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return roleNames;
        }

        //------------------------------------------------------------------------------------
        //                              GetUserId                                             
        //------------------------------------------------------------------------------------
        public string GetUserId(string employeeId)
        {
            string userId = string.Empty;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand command = new SqlCommand("spGetUserId", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@idEmployee", string.IsNullOrEmpty(employeeId) ? DBNull.Value : employeeId));

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            userId = dataReader["Id"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return userId;
        }
        //------------------------------------------------------------------------------------
        //                              GetUserEmails                                             
        //------------------------------------------------------------------------------------
        public List<string> GetUserEmails()
        {
            List<string> userEmails = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

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
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            finally
            {
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

                        command.Parameters.Add(new SqlParameter("@idEmployee", string.IsNullOrEmpty(newEmployee.IdEmployee) ? DBNull.Value : newEmployee.IdEmployee));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newEmployee.Name) ? DBNull.Value : newEmployee.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newEmployee.LastName1) ? DBNull.Value : newEmployee.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newEmployee.LastName2) ? DBNull.Value : newEmployee.LastName2));
                        command.Parameters.Add(new SqlParameter("@position", string.IsNullOrEmpty(newEmployee.Position) ? DBNull.Value : newEmployee.Position));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newEmployee.PhoneNumber) ? DBNull.Value : newEmployee.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idBusiness", newEmployee.IdBusiness != 0 ? (object)newEmployee.IdBusiness : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@businessName", string.IsNullOrEmpty(newEmployee.BusinessName) ? DBNull.Value : newEmployee.BusinessName));
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
                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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

                        command.Parameters.Add(new SqlParameter("@idEmployee", string.IsNullOrEmpty(newEmployee.IdEmployee) ? DBNull.Value : newEmployee.IdEmployee));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newEmployee.Name) ? DBNull.Value : newEmployee.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newEmployee.LastName1) ? DBNull.Value : newEmployee.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newEmployee.LastName2) ? DBNull.Value : newEmployee.LastName2));
                        command.Parameters.Add(new SqlParameter("@position", string.IsNullOrEmpty(newEmployee.Position) ? DBNull.Value : newEmployee.Position));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newEmployee.PhoneNumber) ? DBNull.Value : newEmployee.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idBusiness", newEmployee.IdBusiness != 0 ? (object)newEmployee.IdBusiness : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@businessName", string.IsNullOrEmpty(newEmployee.BusinessName) ? DBNull.Value : newEmployee.BusinessName));
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

                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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
                            new SqlParameter("@businessName", DBNull.Value),
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
                                    Role = dataReader["roleName"].ToString(),
                                };

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }

            return employee;
        }

        //------------------------------------------------------------------------------------
        public Employee GetEmployeeByEmail(string email)
        {
            Employee employee = new Employee();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetEmployeeByEmail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@email", email)
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
                                    IdBusiness =Convert.ToInt32(dataReader["IdBusiness"]),
                                    Email = dataReader["email"].ToString()
                                };

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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
                            new SqlParameter("@businessName", DBNull.Value),
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

                if (sqlEx.Number == 50000)
                {
                    result.Success = false;
                    result.Message = sqlEx.Message;
                    return result;
                }
                else
                {
                    throw new CustomDataException("An error occurred: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }

            return result;
        }
    }
}
