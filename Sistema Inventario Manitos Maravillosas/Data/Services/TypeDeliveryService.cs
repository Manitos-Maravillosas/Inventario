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
        OperationResult Add(TypePayment newTypePayment);
        TypePayment GetById(int id);
        OperationResult Delete(string id);
        OperationResult Update(TypePayment newTypePayment);
        List<string> GetTypePayments();

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
        public OperationResult Add(TypePayment newTypePayment)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypePayment", newTypePayment.Id != 0 ? (object)newTypePayment.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newTypePayment.Name) ? DBNull.Value : newTypePayment.Name));
                        command.Parameters.Add(new SqlParameter("@coinDescription", string.IsNullOrEmpty(newTypePayment.CoinDescription) ? DBNull.Value : newTypePayment.CoinDescription));
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
        public TypePayment GetById(int id)
        {
            TypePayment typePayment = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idTypePayment", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                typePayment = new TypePayment
                                {
                                    Id = Convert.ToInt32(dataReader["idTypePayment"]),
                                    Name = dataReader["name"].ToString(),
                                    CoinDescription = dataReader["coinDescription"].ToString(),

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

            return typePayment;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(TypePayment newTypePayment)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypePayment", newTypePayment.Id != 0 ? (object)newTypePayment.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newTypePayment.Name) ? DBNull.Value : newTypePayment.Name));
                        command.Parameters.Add(new SqlParameter("@coinDescription", string.IsNullOrEmpty(newTypePayment.CoinDescription) ? DBNull.Value : newTypePayment.CoinDescription));
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

        //------------------------------------------------------------------------------------
        //                              GetTypePayments                                             
        //------------------------------------------------------------------------------------
        public List<string> GetTypePayments()
        {
            List<string> TypePaymentNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand GetTypePaymentsCommand = new SqlCommand("spGetTypePaymentNames", connection))
                {
                    GetTypePaymentsCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader typePaymentDataReader = GetTypePaymentsCommand.ExecuteReader())
                    {
                        while (typePaymentDataReader.Read())
                        {
                            string typePaymentName = typePaymentDataReader["name"].ToString();
                            TypePaymentNames.Add(typePaymentName);
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
            return TypePaymentNames;
        }



    }
}
