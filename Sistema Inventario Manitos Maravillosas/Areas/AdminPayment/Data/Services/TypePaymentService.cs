using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Data.Services
{
    public interface ITypePaymentService
    {
        List<TypePaymentxCoin> GetAll();
        OperationResult Add(TypePaymentxCoin newTypePayment);
        TypePaymentxCoin GetById(int id);
        OperationResult Delete(string id);
        OperationResult Update(TypePaymentxCoin newTypePayment);
        List<string> GetTypePayments();
        int GetIdTypePaymentxCoin(int idTypePayment, int idCoin);
        public List<TypePayment> GetAllTypePayments();
    }


    public class TypePaymentService : ITypePaymentService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public TypePaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<TypePaymentxCoin> GetAll()
        {
            List<TypePaymentxCoin> typePayments = new List<TypePaymentxCoin>();
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
                            new SqlParameter("@idTypePaymentxCoin", DBNull.Value),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                TypePaymentxCoin typePayment = new TypePaymentxCoin
                                {
                                    Id = Convert.ToInt32(dataReader["idTypePaymentxCoin"]),
                                    
                                    Name = dataReader["name"].ToString(),
                                    CoinDescription = dataReader["coinDescription"].ToString(),
                                    CoinName = dataReader["coinName"].ToString(),
                                };
                                typePayments.Add(typePayment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                typePayments.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return typePayments;
        }

        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(TypePaymentxCoin newTypePayment)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypePaymentxCoin", newTypePayment.Id != 0 ? newTypePayment.Id : DBNull.Value));
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
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idTypePaymentxCoin", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
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
        public TypePaymentxCoin GetById(int id)
        {
            TypePaymentxCoin typePayment = null;
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
                            new SqlParameter("@idTypePaymentxCoin", id),
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
                                typePayment = new TypePaymentxCoin
                                {
                                    Id = Convert.ToInt32(dataReader["idTypePaymentxCoin"]),
                                    Name = dataReader["name"].ToString(),
                                    CoinDescription = dataReader["coinDescription"].ToString(),
                                    CoinName = dataReader["coinName"].ToString(),
                                    idTypePayment = Convert.ToInt32(dataReader["idTypePayment"]),

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
        public OperationResult Update(TypePaymentxCoin newTypePayment)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spTypePaymentCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idTypePaymentxCoin", newTypePayment.Id));
                        command.Parameters.Add(new SqlParameter("@idTypePayment", newTypePayment.idTypePayment));
                        command.Parameters.Add(new SqlParameter("@name", newTypePayment.Name));
                        command.Parameters.Add(new SqlParameter("@coinDescription", newTypePayment.CoinDescription));
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
        public List<TypePayment> GetAllTypePayments()
        {
            List<TypePayment> typePayment = new List<TypePayment>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand GetTypePaymentsCommand = new SqlCommand("spGetTypePayment", connection))
                {
                    GetTypePaymentsCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader typePaymentDataReader = GetTypePaymentsCommand.ExecuteReader())
                    {
                        while (typePaymentDataReader.Read())
                        {
                            TypePayment typePaymentName = new TypePayment
                            {
                                IdTypePayment = Convert.ToInt32(typePaymentDataReader["idTypePayment"]),
                                Name = typePaymentDataReader["name"].ToString(),
                            };
                            typePayment.Add(typePaymentName);
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
            return typePayment;
        }

        public int GetIdTypePaymentxCoin(int idTypePayment, int idCoin)
        {
           int typePaymentxCoin = 0;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand command = new SqlCommand("spGetIdTypePaymentxCoin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    
                    command.Parameters.Add(new SqlParameter("@idTypePayment", idTypePayment));
                    command.Parameters.Add(new SqlParameter("@idCoin", idCoin));

                    connection.Open();
                    command.ExecuteNonQuery();

                    using (SqlDataReader typePaymentDataReader = command.ExecuteReader())
                    {
                        while (typePaymentDataReader.Read())
                        {
                            typePaymentxCoin = Convert.ToInt32(typePaymentDataReader["idTypePaymentxCoin"]);
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
            return typePaymentxCoin;
        }
    }
}
