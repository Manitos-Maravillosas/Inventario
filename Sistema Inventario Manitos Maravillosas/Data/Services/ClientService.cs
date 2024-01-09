using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{

    public interface IClientService
    {
        List<Client> GetAll();
        Client GetById(string id);
        OperationResult Add(Client newClient);
        OperationResult Update(Client newClient);
        OperationResult Delete(string id);
    }

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
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newClient.PhoneNumber) ? DBNull.Value : newClient.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idAddress", newClient.IdAddress != 0 ? (object)newClient.IdAddress : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@signs", string.IsNullOrEmpty(newClient.Signs) ? DBNull.Value : newClient.Signs));
                        command.Parameters.Add(new SqlParameter("@cityName", string.IsNullOrEmpty(newClient.CityName) ? DBNull.Value : newClient.CityName));
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
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }


            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@idAddress", DBNull.Value),
                            new SqlParameter("@signs", DBNull.Value),
                            new SqlParameter("@cityName", DBNull.Value),
                            new SqlParameter("@departmentName", DBNull.Value),
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
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }


            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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
                                    Signs = dataReader["signs"].ToString(),

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
                throw new CustomDataException(ex.Message, ex);
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
                            new SqlParameter("@signs", DBNull.Value),
                            new SqlParameter("@cityName", DBNull.Value),
                            new SqlParameter("@departmentName", DBNull.Value),
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
                                Signs = dataReader["signs"].ToString(),
                                CityName = dataReader["cityName"].ToString(),
                                DepartmentName = dataReader["departmentName"].ToString(),
                            };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred while fetching the client by ID.", ex);
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
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newClient.PhoneNumber) ? DBNull.Value : newClient.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@idAddress", newClient.IdAddress != 0 ? (object)newClient.IdAddress : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@signs", string.IsNullOrEmpty(newClient.Signs) ? DBNull.Value : newClient.Signs));
                        command.Parameters.Add(new SqlParameter("@cityName", string.IsNullOrEmpty(newClient.CityName) ? DBNull.Value : newClient.CityName));
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
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
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
