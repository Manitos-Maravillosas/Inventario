using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface ITypeDeliveryService
    {
        List<TypeDelivery> GetAll();
        OperationResult Add(TypeDelivery newTypeDelivery);
        TypeDelivery GetById(int id);
        OperationResult Delete(string id);
        OperationResult Update(TypeDelivery newTypeDelivery);

    }

    public class TypeDeliveryService : ITypeDeliveryService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public TypeDeliveryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<TypeDelivery> GetAll()
        {
            List<TypeDelivery> typeDeliveries = new List<TypeDelivery>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypeDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idTypeDelivery", DBNull.Value),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                TypeDelivery typeDelivery = new TypeDelivery
                                {
                                    Id = Convert.ToInt32(dataReader["idTypeDelivery"]),
                                    Name = dataReader["name"].ToString(),
                                    Description = dataReader["description"].ToString(),
                                };
                                typeDeliveries.Add(typeDelivery);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                typeDeliveries.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return typeDeliveries;
        }

        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(TypeDelivery newTypeDelivery)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypeDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypeDelivery", newTypeDelivery.Id != 0 ? (object)newTypeDelivery.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newTypeDelivery.Name) ? DBNull.Value : newTypeDelivery.Name));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(newTypeDelivery.Description) ? DBNull.Value : newTypeDelivery.Description));
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
                    using (SqlCommand command = new SqlCommand("spTypeDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idTypeDelivery", id),
                            new SqlParameter("@name", DBNull.Value),
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
        public TypeDelivery GetById(int id)
        {
            TypeDelivery typeDelivery = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypeDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idTypeDelivery", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                typeDelivery = new TypeDelivery
                                {
                                    Id = Convert.ToInt32(dataReader["idTypeDelivery"]),
                                    Name = dataReader["name"].ToString(),
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

            return typeDelivery;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(TypeDelivery newTypeDelivery)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypeDeliveryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypeDelivery", newTypeDelivery.Id != 0 ? (object)newTypeDelivery.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newTypeDelivery.Name) ? DBNull.Value : newTypeDelivery.Name));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(newTypeDelivery.Description) ? DBNull.Value : newTypeDelivery.Description));
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
