using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IProviderService
    {
        List<Provider> GetAll();
        OperationResult Add(Provider newProvider);
        OperationResult Delete(string id);
        Provider GetById(int id);       
        OperationResult Update(Provider newProvider);
    }

    public class ProviderService : IProviderService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public ProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Provider> GetAll()
        {
            List<Provider> providers = new List<Provider>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProvider", DBNull.Value),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Provider provider = new Provider
                                {
                                    Id = Convert.ToInt32(dataReader["idProvider"]),
                                    Name = dataReader["name"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    Description = dataReader["description"].ToString(),
                                };
                                providers.Add(provider);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                providers.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return providers;
        }

        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(Provider newProvider)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProvider", newProvider.Id != 0 ? (object)newProvider.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newProvider.Name) ? DBNull.Value : newProvider.Name));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newProvider.PhoneNumber) ? DBNull.Value : newProvider.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(newProvider.Description) ? DBNull.Value : newProvider.Description));
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
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProvider", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
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
        //                              GetById                                             
        //------------------------------------------------------------------------------------
        public Provider GetById(int id)
        {
            Provider provider = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProvider", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                provider = new Provider
                                {
                                    Id = Convert.ToInt32(dataReader["idProvider"]),
                                    Name = dataReader["name"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    Description = dataReader["description"].ToString(),

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred while fetching the type payment by ID.", ex);
            }

            return provider;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(Provider newProvider)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProvider", newProvider.Id != 0 ? (object)newProvider.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newProvider.Name) ? DBNull.Value : newProvider.Name));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newProvider.PhoneNumber) ? DBNull.Value : newProvider.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(newProvider.Description) ? DBNull.Value : newProvider.Description));
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
