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



    }
}
