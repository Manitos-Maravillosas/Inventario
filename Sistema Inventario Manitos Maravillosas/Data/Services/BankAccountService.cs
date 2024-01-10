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



    }
}
