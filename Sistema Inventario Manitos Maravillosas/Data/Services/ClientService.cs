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
                if (sqlEx.Number == 50000) 
                {
                    result.Success = false;
                    result.Message = sqlEx.Message; 
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
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@idAddress", DBNull.Value),
                            new SqlParameter("@operation", '2') 
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
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    AddressName = dataReader["signs"].ToString(),

                                };
                                clients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clients.Clear(); 
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
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@idAddress", DBNull.Value),
                            new SqlParameter("@operation", '2') 
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            
                            while (dataReader.Read())
                            {   client = new Client
                                {
                                    Id = dataReader["idClient"].ToString(),
                                    Name = dataReader["name"].ToString(),
                                    LastName1 = dataReader["lastName1"].ToString(),
                                    LastName2 = dataReader["lastName2"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    AddressName = dataReader["signs"].ToString(),
                            };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

                if (sqlEx.Number == 50000) 
                {
                    result.Success = false;
                    result.Message = sqlEx.Message; 
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
