using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public class ClientService : IClientService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public ClientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(Client newClient)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spClientCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Configure parameters
                        command.Parameters.Add(new SqlParameter("@idClient", string.IsNullOrEmpty(newClient.Id) ? DBNull.Value : newClient.Id));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newClient.Name) ? DBNull.Value : newClient.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newClient.LastName1) ? DBNull.Value : newClient.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newClient.LastName2) ? DBNull.Value : newClient.LastName2));
                        command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(newClient.Email) ? DBNull.Value : newClient.Email));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newClient.PhoneNumber) ? DBNull.Value : newClient.PhoneNumber));
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
        //                              Delete                                             
        //------------------------------------------------------------------------------------
        public OperationResult Delete(string id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spClientCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idClient", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@lastName1", DBNull.Value),
                            new SqlParameter("@lastName2", DBNull.Value),
                            new SqlParameter("@email", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@operation", '4') // Operation for 'Read' is 2
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
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Client> GetAll()
        {
            List<Client> clients = new List<Client>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spClientCRUD", connection))
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
                            new SqlParameter("@operation", '2') // Operation for 'Read' is 2
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (!dataReader.HasRows)
                            {
                                // Handle the case when no data is returned
                                // You might want to log this or handle it according to your application's logic
                                return null; // Return the empty list
                            }

                            while (dataReader.Read())
                            {
                                Client client = new Client
                                {
                                    Id = dataReader["idClient"].ToString(),
                                    Name = dataReader["name"].ToString(),
                                    LastName1 = dataReader["lastName1"].ToString(),
                                    LastName2 = dataReader["lastName2"].ToString(),
                                    Email = dataReader["email"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString()
                                };
                                clients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                clients.Clear(); // This will return an empty list in case of an error.
            }

            return clients;
        }
        //------------------------------------------------------------------------------------
        //                              GetById                                             
        //------------------------------------------------------------------------------------
        public Client GetById(string id)
        {
            Client client = new Client();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spClientCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                    new SqlParameter("@idClient", id),
                    new SqlParameter("@name", DBNull.Value),
                    new SqlParameter("@lastName1", DBNull.Value),
                    new SqlParameter("@lastName2", DBNull.Value),
                    new SqlParameter("@email", DBNull.Value),
                    new SqlParameter("@phoneNumber", DBNull.Value),
                    new SqlParameter("@operation", '2') // Operation for 'Read' is 2
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (!dataReader.HasRows)
                            {
                                // Handle the case when no data is returned
                                // You might want to log this or handle it according to your application's logic
                                return null; // Return the empty list
                            }

                            while (dataReader.Read())
                            {   client = new Client
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
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return client;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(Client newClient)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spClientCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Configure parameters
                        command.Parameters.Add(new SqlParameter("@idClient", string.IsNullOrEmpty(newClient.Id) ? DBNull.Value : newClient.Id));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newClient.Name) ? DBNull.Value : newClient.Name));
                        command.Parameters.Add(new SqlParameter("@lastName1", string.IsNullOrEmpty(newClient.LastName1) ? DBNull.Value : newClient.LastName1));
                        command.Parameters.Add(new SqlParameter("@lastName2", string.IsNullOrEmpty(newClient.LastName2) ? DBNull.Value : newClient.LastName2));
                        command.Parameters.Add(new SqlParameter("@email", string.IsNullOrEmpty(newClient.Email) ? DBNull.Value : newClient.Email));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newClient.PhoneNumber) ? DBNull.Value : newClient.PhoneNumber));
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
    }
}
