using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IBankAccountService
    {
        List<BankAccount> GetAll();
        OperationResult Delete(string id);
        List<string> GetBankNames();
        OperationResult Add(BankAccount newBankAccount);
        BankAccount GetById(int id);
        OperationResult Update(BankAccount newBankAccount);
    }

    public class BankAccountService : IBankAccountService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public BankAccountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<BankAccount> GetAll()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spBankAccountCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idBankAccount", DBNull.Value),
                            new SqlParameter("@accountNumber", DBNull.Value),
                            new SqlParameter("@bankName", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
                            new SqlParameter("@typePaymentName", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                BankAccount bankAccount = new BankAccount
                                {
                                    Id = Convert.ToInt32(dataReader["iBankAccount"]),
                                    AccountNumber = dataReader["accountNumber"].ToString(),
                                    BankName = dataReader["bankName"].ToString(),
                                    CoinDescription = dataReader["coinDescription"].ToString(),
                                    TypePaymentName = dataReader["typePaymentName"].ToString(),
                                };
                                bankAccounts.Add(bankAccount);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bankAccounts.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return bankAccounts;
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
                    using (SqlCommand command = new SqlCommand("spBankAccountCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idBankAccount", id),
                            new SqlParameter("@accountNumber", DBNull.Value),
                            new SqlParameter("@bankName", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
                            new SqlParameter("@typePaymentName", DBNull.Value),
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
        //                              GetBankNmaes                                             
        //------------------------------------------------------------------------------------
        public List<string> GetBankNames()
        {
            List<string> BankNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand GetBankNamesCommand = new SqlCommand("spGetBankNames", connection))
                {
                    GetBankNamesCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader bankNameDataReader = GetBankNamesCommand.ExecuteReader())
                    {
                        while (bankNameDataReader.Read())
                        {
                            string bankName = bankNameDataReader["name"].ToString();
                            BankNames.Add(bankName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener descripciones de monedas.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return BankNames;
        }

        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(BankAccount newBankAccount)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spBankAccountCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idBankAccount", newBankAccount.Id != 0 ? (object)newBankAccount.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@accountNumber", string.IsNullOrEmpty(newBankAccount.AccountNumber) ? DBNull.Value : newBankAccount.AccountNumber));
                        command.Parameters.Add(new SqlParameter("@bankName", string.IsNullOrEmpty(newBankAccount.BankName) ? DBNull.Value : newBankAccount.BankName));
                        command.Parameters.Add(new SqlParameter("@typePaymentName", string.IsNullOrEmpty(newBankAccount.TypePaymentName) ? DBNull.Value : newBankAccount.TypePaymentName));
                        command.Parameters.Add(new SqlParameter("@coinDescription", string.IsNullOrEmpty(newBankAccount.CoinDescription) ? DBNull.Value : newBankAccount.CoinDescription));
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
        //                              GetById                                             
        //------------------------------------------------------------------------------------
        public BankAccount GetById(int id)
        {
            BankAccount bankAccount = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spBankAccountCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idBankAccount", id),
                            new SqlParameter("@accountNumber", DBNull.Value),
                            new SqlParameter("@bankName", DBNull.Value),
                            new SqlParameter("@coinDescription", DBNull.Value),
                            new SqlParameter("@typePaymentName", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                bankAccount = new BankAccount
                                {
                                    Id = Convert.ToInt32(dataReader["iBankAccount"]),
                                    AccountNumber = dataReader["accountNumber"].ToString(),
                                    BankName = dataReader["bankName"].ToString(),
                                    CoinDescription = dataReader["coinDescription"].ToString(),
                                    TypePaymentName = dataReader["typePaymentName"].ToString(),

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

            return bankAccount;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(BankAccount newBankAccount)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spBankAccountCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idBankAccount", newBankAccount.Id != 0 ? (object)newBankAccount.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@accountNumber", string.IsNullOrEmpty(newBankAccount.AccountNumber) ? DBNull.Value : newBankAccount.AccountNumber));
                        command.Parameters.Add(new SqlParameter("@bankName", string.IsNullOrEmpty(newBankAccount.BankName) ? DBNull.Value : newBankAccount.BankName));
                        command.Parameters.Add(new SqlParameter("@typePaymentName", string.IsNullOrEmpty(newBankAccount.TypePaymentName) ? DBNull.Value : newBankAccount.TypePaymentName));
                        command.Parameters.Add(new SqlParameter("@coinDescription", string.IsNullOrEmpty(newBankAccount.CoinDescription) ? DBNull.Value : newBankAccount.CoinDescription));
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
