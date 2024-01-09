using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IAddressService
    {
        List<string> GetDepartmentNames();
        List<string> GetCitiesByDepartmentName(string idDepartment);
    }
    public class AddressService : IAddressService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public AddressService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        //------------------------------------------------------------------------------------
        //                              GetDeparments                                             
        //------------------------------------------------------------------------------------
        public List<string> GetDepartmentNames()
        {
            List<string> departmentNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                using (SqlCommand getDepartmentsCommand = new SqlCommand("spGetDepartment", connection))
                {
                    getDepartmentsCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader departmentDataReader = getDepartmentsCommand.ExecuteReader())
                    {
                        while (departmentDataReader.Read())
                        {
                            string departmentName = departmentDataReader["name"].ToString();
                            departmentNames.Add(departmentName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener nombres de departamentos.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return departmentNames;
        }

        //------------------------------------------------------------------------------------
        //                              GetCityNamesByDepartment                                             
        //------------------------------------------------------------------------------------
        public List<string> GetCitiesByDepartmentName(string departmentName)
        {
            List<string> cityNames = new List<string>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("spGetCityByDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@departmentName", departmentName);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string cityName = reader["name"].ToString();
                            cityNames.Add(cityName);
                        }
                    }
                }
            }
            return cityNames;
        }
    }
}
