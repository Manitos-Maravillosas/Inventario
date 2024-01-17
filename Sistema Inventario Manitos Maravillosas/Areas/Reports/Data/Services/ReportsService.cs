using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Data.Services
{
    public interface IReportsService
    {
        //totalSales
        public List<TotalSalesModel> GetTotalSales(DateTime startDate, DateTime endDate);
    }

    public class ReportsService : IReportsService
    {
        private readonly IConfiguration _configuration;

        public ReportsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<TotalSalesModel> GetTotalSales(DateTime startDate, DateTime endDate)
        {
            List<TotalSalesModel> sales = new List<TotalSalesModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spTotalSales", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@startDate", startDate),
                        new SqlParameter("@endDate", endDate)
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            TotalSalesModel sale = new TotalSalesModel
                            {
                                IdBill = Convert.ToInt32(dataReader["idBill"]),
                                Date = Convert.ToDateTime(dataReader["date"]),
                                PercentDiscount = Convert.ToSingle(dataReader["percentDicount"]),
                                SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                EmployeeName = Convert.ToString(dataReader["employeeName"]),
                                ClientName = Convert.ToString(dataReader["clientName"]),
                                BusinessName = Convert.ToString(dataReader["businessName"])
                            };
                            sales.Add(sale);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return sales;
        }
    }
}
