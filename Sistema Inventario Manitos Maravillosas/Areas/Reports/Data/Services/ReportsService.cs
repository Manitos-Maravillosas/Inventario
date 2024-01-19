using Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Models;
using System.Data;
using System.Data.SqlClient;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Reports.Data.Services
{
    public interface IReportsService
    {
        //totalSales
        public List<TotalSalesModel> GetTotalSales(DateTime startDate, DateTime endDate);
        public List<SalesByProductModel> GetSalesByProduct(DateTime startDate, DateTime endDate);
        public List<SalesByProductCategoryModel> GetSalesByProductCategory(DateTime startDate, DateTime endDate);
        public List<SalesByClientModel> GetSalesByClient(DateTime startDate, DateTime endDate);
        public List<SalesByBusinessModel> GetSalesByBusiness(DateTime startDate, DateTime endDate);
        public List<BillsByClientModel> GetBillsByClient(DateTime startDate, DateTime endDate, string idClient);
        public List<BillsByBusinessModel> GetBillsByBusiness(DateTime startDate, DateTime endDate, int idBusiness);
        public FinancialSummary GetFinancialSummary(DateTime startDate, DateTime endDate);
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
                                PercentDiscount = Convert.ToSingle(dataReader["percentDiscount"]),
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
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
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

        public List<SalesByProductModel> GetSalesByProduct(DateTime startDate, DateTime endDate)
        {
            List<SalesByProductModel> sales = new List<SalesByProductModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spSalesByProduct", connection))
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
                            SalesByProductModel sale = new SalesByProductModel
                            {
                                IdProduct = Convert.ToString(dataReader["idProduct"]),
                                ProductName = Convert.ToString(dataReader["productName"]),
                                AmountSold = Convert.ToInt32(dataReader["amountSold"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                TotalSold = Convert.ToSingle(dataReader["totalSold"]),
                                TotalProfit = Convert.ToSingle(dataReader["totalProfit"])
                            };
                            sales.Add(sale);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
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

        public List<SalesByProductCategoryModel> GetSalesByProductCategory(DateTime startDate, DateTime endDate)
        {
            List<SalesByProductCategoryModel> sales = new List<SalesByProductCategoryModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spSalesByProductCategory", connection))
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
                            SalesByProductCategoryModel sale = new SalesByProductCategoryModel
                            {
                                IdCategory = Convert.ToInt32(dataReader["idCategory"]),
                                CategoryName = Convert.ToString(dataReader["CategoryName"]),
                                AmountSold = Convert.ToInt32(dataReader["amountSold"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                TotalSold = Convert.ToSingle(dataReader["totalSold"]),
                                TotalProfit = Convert.ToSingle(dataReader["totalProfit"])
                            };
                            sales.Add(sale);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
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

        public List<SalesByClientModel> GetSalesByClient(DateTime startDate, DateTime endDate)
        {
            List<SalesByClientModel> sales = new List<SalesByClientModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spSalesByClient", connection))
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
                            SalesByClientModel sale = new SalesByClientModel
                            {
                                IdClient = Convert.ToString(dataReader["idClient"]),
                                Name = Convert.ToString(dataReader["name"]),
                                LastName1 = Convert.ToString(dataReader["lastName1"]),
                                LastName2 = Convert.ToString(dataReader["lastName2"]),
                                PhoneNumber = Convert.ToString(dataReader["phoneNumber"]),
                                TotalPurchased = Convert.ToSingle(dataReader["totalPurchased"])
                            };
                            sales.Add(sale);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
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

        public List<SalesByBusinessModel> GetSalesByBusiness(DateTime startDate, DateTime endDate)
        {
            List<SalesByBusinessModel> sales = new List<SalesByBusinessModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spSalesByBusiness", connection))
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
                            SalesByBusinessModel sale = new SalesByBusinessModel
                            {
                                IdBusiness = Convert.ToInt32(dataReader["idBusiness"]),
                                BusinessName = Convert.ToString(dataReader["businessName"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                TotalSold = Convert.ToSingle(dataReader["totalSold"]),
                                TotalProfit = Convert.ToSingle(dataReader["totalProfit"])
                            };
                            sales.Add(sale);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
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

        public List<BillsByClientModel> GetBillsByClient(DateTime startDate, DateTime endDate, string idClient)
        {
            List<BillsByClientModel> bills = new List<BillsByClientModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spBillsByClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@startDate", startDate),
                        new SqlParameter("@endDate", endDate),
                        new SqlParameter("@idClient", idClient)
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            BillsByClientModel bill = new BillsByClientModel
                            {
                                IdBill = Convert.ToInt32(dataReader["idBill"]),
                                Date = Convert.ToDateTime(dataReader["date"]),
                                PercentDiscount = Convert.ToSingle(dataReader["percentDiscount"]),
                                SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                BusinessName = Convert.ToString(dataReader["businessName"])
                            };
                            bills.Add(bill);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return bills;
        }

        public List<BillsByBusinessModel> GetBillsByBusiness(DateTime startDate, DateTime endDate, int idBusiness)
        {
            List<BillsByBusinessModel> bills = new List<BillsByBusinessModel>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spBillsByBusiness", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@startDate", startDate),
                        new SqlParameter("@endDate", endDate),
                        new SqlParameter("@idBusiness", idBusiness)
                    };

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            BillsByBusinessModel bill = new BillsByBusinessModel
                            {
                                IdBill = Convert.ToInt32(dataReader["idBill"]),
                                Date = Convert.ToDateTime(dataReader["date"]),
                                PercentDiscount = Convert.ToSingle(dataReader["percentDiscount"]),
                                SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                ClientName = Convert.ToString(dataReader["clientName"]),
                                EmployeeName = Convert.ToString(dataReader["employeeName"])
                            };
                            bills.Add(bill);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return bills;
        }

        public FinancialSummary GetFinancialSummary(DateTime startDate, DateTime endDate)
        {
            FinancialSummary summary = new FinancialSummary();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                using (SqlCommand command = new SqlCommand("spGetFinancialSummary", connection))
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
                            // Check for null values in data reader
                            summary.TotalExpenses = dataReader["totalExpenses"] == DBNull.Value ? 0 : Convert.ToSingle(dataReader["totalExpenses"]);
                            summary.TotalSales = dataReader["totalSales"] == DBNull.Value ? 0 : Convert.ToSingle(dataReader["totalSales"]);
                            summary.TotalProfit = dataReader["totalProfit"] == DBNull.Value ? 0 : Convert.ToSingle(dataReader["totalProfit"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al leer datos del SqlDataReader.", ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return summary;
        }
    }
}
