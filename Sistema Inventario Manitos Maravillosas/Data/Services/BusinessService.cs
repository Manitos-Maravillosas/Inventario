using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IBusinessService
    {
        List<string> GetBusinessNames();
    }
    public class BusinessService : IBusinessService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public BusinessService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //------------------------------------------------------------------------------------
        //                              GetBusinessNames                                             
        //------------------------------------------------------------------------------------
        public List<string> GetBusinessNames()
        {
            List<string> businessNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getBusinessNamesCommand = new SqlCommand("spGetBusinessNames", connection))
                {
                    getBusinessNamesCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader businessDataReader = getBusinessNamesCommand.ExecuteReader())
                    {
                        while (businessDataReader.Read())
                        {
                            string businessName = businessDataReader["name"].ToString();
                            businessNames.Add(businessName);
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
            return businessNames;
        }

    }
}