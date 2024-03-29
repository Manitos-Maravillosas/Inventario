﻿using Humanizer;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.AdminPayment.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Inventory.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models.Admin;
using System.Data;
using System.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services
{
    public interface IBillService
    {
        DataTable ConvertToDataTable(List<CartXProduct> products);
        DataTable ConverToTabaTableDelivery(Delivery delivery);
        DataTable ConverToTabaBillxTypePayment(BillxTypePayment billxTypePayment);
        Boolean SaveBill(Bill bill);
        List<Bill> GetAll();

        Bill GetBillById(int id);

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
                                    PercentDiscount = Convert.ToSingle(dataReader["percentDiscount"]),
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
        public Bill GetBillById(int id)
        {
            Bill bill = new Bill();
            List<CartXProduct> cartXProducts = new List<CartXProduct>();
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
                            new SqlParameter("@idBill", id),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);
                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Bill b = new Bill
                                {
                                    IdBill = Convert.ToInt32(dataReader["idBill"]),
                                    Date = Convert.ToDateTime(dataReader["date"]).Date,
                                    PercentDiscount = Convert.ToSingle(dataReader["percentDiscount"]),
                                    SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                    TotalCost = Convert.ToSingle(dataReader["totalCost"]),
                                    BusinessName = dataReader["businessName"].ToString(),
                                    IdBusiness = Convert.ToInt32(dataReader["idBusiness"]),
                                    IdClient = dataReader["idClient"].ToString(),
                                    IdEmployee = dataReader["idEmployee"].ToString(),
                                    deliveryFlag = Convert.ToBoolean(dataReader["isDelivery"]),
                                    idDelivery = Convert.ToInt32(dataReader["idDelivery"]),
                                };
                                bill = b;
                            }
                        }
                        command.CommandText = "spGetProductsOfBill";
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@idBill", id));

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                CartXProduct cart = new CartXProduct
                                {
                                    Quantity = Convert.ToInt32(dataReader["cant"]),
                                    Cost = Convert.ToSingle(dataReader["cost"]),
                                    Price = Convert.ToSingle(dataReader["price"]),
                                    SubTotal = Convert.ToSingle(dataReader["subTotal"]),
                                    IdProduct = dataReader["idProduct"].ToString(),
                                    //products
                                };
                                cart.Product = new ProductFacturation
                                {
                                    IdProduct = dataReader["idProduct"].ToString(),
                                    ProductName = dataReader["name"].ToString(),
                                    Category = dataReader["category"].ToString(),
                                    Description = dataReader["description"].ToString(),
                                };

                                cartXProducts.Add(cart);
                            }
                            bill.CartXProducts = cartXProducts;
                        }
                    }
                }
            }      
            catch (Exception ex)
            {
                throw new CustomDataException(ex.Message, ex);
            }

            return bill;
        }
        public DataTable ConvertToDataTable(List<CartXProduct> products)
        {
            DataTable table = new DataTable();
            table.Columns.Add("cant", typeof(int));
            table.Columns.Add("cost", typeof(float));
            table.Columns.Add("price", typeof(float));
            table.Columns.Add("subTotal", typeof(float));
            table.Columns.Add("idProduct", typeof(string));

            foreach (var product in products)
            {
                table.Rows.Add(product.Quantity, product.Cost, product.Price, product.SubTotal, product.IdProduct);
            }

            return table;
        }

        public DataTable ConverToTabaTableDelivery(Delivery delivery)
        {
            DataTable table = new DataTable();
            table.Columns.Add("total", typeof(float));
            table.Columns.Add("internalCost", typeof(float));
            table.Columns.Add("dateAprox", typeof(DateTime));
            table.Columns.Add("notes", typeof(string));
            table.Columns.Add("idAddres", typeof(int));
            table.Columns.Add("idTypeDelivery", typeof(int));
            table.Columns.Add("status", typeof(Boolean));


            //data for delivery to external
            table.Columns.Add("companyCost", typeof(int));
            table.Columns.Add("idInChargePayment", typeof(int));
            table.Columns.Add("idCompanyTrans", typeof(int));

            //add rows 
            table.Rows.Add(delivery.Total, delivery.InternalCost, delivery.DateAprox, delivery.Notes, delivery.IdAddress, delivery.IdTypeDelivery, true,
                delivery.deliveryxCompanyTrans.AditionalCompanyCost, delivery.deliveryxCompanyTrans.InChargePaymentDelivery, delivery.deliveryxCompanyTrans.IdCompanyTrans);

            return table;
        }
        public DataTable ConverToTabaBillxTypePayment(BillxTypePayment billxTypePayment)
        {
            DataTable table = new DataTable();
            table.Columns.Add("idTypePaymentXCoin", typeof(int));
            table.Columns.Add("amount", typeof(float));

            if (billxTypePayment.bothCoins)
            {
                table.Rows.Add(5, billxTypePayment.amountPaidDolar);
                table.Rows.Add(1, billxTypePayment.amountPaidCordoba);
            }
            else
            {
                table.Rows.Add(billxTypePayment.typePaymentxCoin.Id, billxTypePayment.amountPaid);
            }
            return table;
        }

        public Boolean SaveBill(Bill bill)
        {
            ProductFacturation product = new ProductFacturation();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spSaveBill", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@date", bill.Date));
                        command.Parameters.Add(new SqlParameter("@percentDiscount", bill.PercentDiscount));
                        command.Parameters.Add(new SqlParameter("@subTotal", bill.SubTotal));
                        command.Parameters.Add(new SqlParameter("@totalCost", bill.TotalCost));
                        command.Parameters.Add(new SqlParameter("@idEmployee", bill.Employee.IdEmployee));
                        command.Parameters.Add(new SqlParameter("@idClient", bill.Client.Id));
                        command.Parameters.Add(new SqlParameter("@idBusiness", bill.IdBusiness));
                        command.Parameters.Add(new SqlParameter("@isDelivery", bill.deliveryFlag));
                        command.Parameters.Add(new SqlParameter("@productList", ConvertToDataTable(bill.CartXProducts)));
                        command.Parameters.Add(new SqlParameter("@deliveryInfo", ConverToTabaTableDelivery(bill.delivery)));
                        command.Parameters.Add(new SqlParameter("@billxTypePaymentTypeInfo", ConverToTabaBillxTypePayment(bill.billxTypePayment)));
                       
                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader != null)
                            {
                                // Handle the case when no data is returned
                                // You might want to log this or handle it according to your application's logic
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Check if the error is a custom error thrown using RAISEERROR
                if (sqlEx.Number == 50000) // 50000 is the default error number for RAISEERROR
                {
                    throw new CustomDataException("Sql", sqlEx);
                }
                else
                {
                    // You can use different strategies to relay this message back to the user.
                    // For example, you might throw a new exception with the user-friendly message,
                    // or you could return an error response that your frontend can use to display the alert.
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Consider whether to throw a custom exception or handle it differently
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }
            return true;
        }


        public ProductFacturation GetStockById(string id, int quantity)
        {
            ProductFacturation product = new ProductFacturation();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spGetStockOfProductById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProduct",id),
                            new SqlParameter("@cantProduct", quantity)
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (!dataReader.HasRows)
                            {
                                // Handle the case when no data is returned
                                // You might want to log this or handle it according to your application's logic
                                return null; // Return the empty list
                            }

                            while (dataReader.Read())
                            {
                                product.IdProduct = Convert.ToString(dataReader["IdProduct"]);
                                product.ProductName = Convert.ToString(dataReader["name"]);
                                product.Stock = Convert.ToInt32(dataReader["Stock"]);
                                product.Cost = Convert.ToSingle(dataReader["Cost"]);
                                product.Price = Convert.ToSingle(dataReader["Price"]);
                                product.Description = Convert.ToString(dataReader["Description"]);
                                product.Status = Convert.ToBoolean(dataReader["Status"]);
                                product.IdBusiness = Convert.ToInt32(dataReader["IdBusiness"]);
                                product.IdProductCategory = Convert.ToInt32(dataReader["IdProductCategory"]);
                                product.Category = Convert.ToString(dataReader["Category"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Check if the error is a custom error thrown using RAISEERROR
                if (sqlEx.Number == 50000) // 50000 is the default error number for RAISEERROR
                {
                    throw new CustomDataException("Sql", sqlEx);
                }
                else
                {
                    // You can use different strategies to relay this message back to the user.
                    // For example, you might throw a new exception with the user-friendly message,
                    // or you could return an error response that your frontend can use to display the alert.
                    throw new CustomDataException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Consider whether to throw a custom exception or handle it differently
                throw new CustomDataException("An error occurred: " + ex.Message, ex);
            }

            return product;
        }
    }
}
