using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface ICoinService
    {
        public List<string> GetCoinDescriptions();

        public string GetNameFromDescription(string coinDescription);
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
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
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

        public string GetNameFromDescription(string coinDescription)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            string name = "";
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand command = new SqlCommand("spGetNameFromDescription", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {                          
                        new SqlParameter("@coinDescription", coinDescription)
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();


                    using (SqlDataReader coinDataReader = command.ExecuteReader())
                    {
                        while (coinDataReader.Read())
                        {
                            name = coinDataReader["name"].ToString();
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
            return name;
        }
    }
}
