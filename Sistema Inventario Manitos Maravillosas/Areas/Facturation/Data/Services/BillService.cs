using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services
{
    public interface IBillService
    {
        List<Bill> GetAll();

    }
    public class BillService : IBillService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public BillService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Bill> GetAll()
        {
            List<Bill> bills = new List<Bill>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spBillCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idBill", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Bill bill = new Bill
                                {
                                    IdBill = Convert.ToInt32(dataReader["idBill"]),
                                    Date = Convert.ToDateTime(dataReader["date"]).Date,
                                    PercentDiscount = Convert.ToSingle(dataReader["percentDicount"]),
                                    SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                    TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                    BusinessName = dataReader["businessName"].ToString(),
                                };
                                bills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bills.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return bills;
        }
    }
}
