using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface ICoinService
    {
        List<string> GetCoinDescriptions();
    }
    public class CoinService : ICoinService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public CoinService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //------------------------------------------------------------------------------------
        //                              GetCoinDescriptions                                             
        //------------------------------------------------------------------------------------
        public List<string> GetCoinDescriptions()
        {
            List<string> coinDescriptions = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getCoinDescriptionsCommand = new SqlCommand("spGetCoinDescriptions", connection))
                {
                    getCoinDescriptionsCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader coinDataReader = getCoinDescriptionsCommand.ExecuteReader())
                    {
                        while (coinDataReader.Read())
                        {
                            string coinDescription = coinDataReader["description"].ToString();
                            coinDescriptions.Add(coinDescription);
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
            return coinDescriptions;
        }


    }
}
