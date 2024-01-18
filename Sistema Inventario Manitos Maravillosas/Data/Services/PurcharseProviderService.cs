using Microsoft.IdentityModel.Tokens;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Supplier.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Sistema_Inventario_Manitos_Maravillosas.Data.Services
{
    public interface IPurchaseProviderService
    {
        List<PurchaseProvider> GetAll();
        OperationResult Add(PurchaseProvider newPurchaseProvider);        
    }

    public class PurchaseProviderService : IPurchaseProviderService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public PurchaseProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<PurchaseProvider> GetAll()
        {
            List<PurchaseProvider> purchases = new List<PurchaseProvider>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spPurchaseInventoryCRUD ", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idPurchaseInventory", DBNull.Value),
                            new SqlParameter("@date", DBNull.Value),
                            new SqlParameter("@cant", DBNull.Value),
                            new SqlParameter("@cost", DBNull.Value),
                            new SqlParameter("@total", DBNull.Value),
                            new SqlParameter("@productName", DBNull.Value),
                            new SqlParameter("@providerName", DBNull.Value),
                            new SqlParameter("@businessName", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                PurchaseProvider purchase = new PurchaseProvider
                                {
                                    Id = Convert.ToInt32(dataReader["idPurchaseInventory"]),
                                    Date = Convert.ToDateTime(dataReader["date"]).Date,
                                    ProductName = dataReader["productName"].ToString(),
                                    Cant = Convert.ToInt32(dataReader["cant"]),
                                    Cost = Convert.ToSingle(dataReader["cost"]),
                                    Total = Convert.ToSingle(dataReader["total"]),
                                    ProviderName = dataReader["providerName"].ToString(),
                                    BusinessName = dataReader["businessName"].ToString(),
                                };
                                purchases.Add(purchase);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                purchases.Clear();
                throw new CustomDataException(ex.Message, ex);
            }

            return purchases;
        }

        //------------------------------------------------------------------------------------
        //                              Add                                             
        //------------------------------------------------------------------------------------
        public OperationResult Add(PurchaseProvider newPurchaseProvider)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spPurchaseInventoryCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idPurchaseInventory", newPurchaseProvider.Id != 0 ? (object)newPurchaseProvider.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@date", newPurchaseProvider.Date != null ? (object)newPurchaseProvider.Date : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@cant", newPurchaseProvider.Cant != null ? (object)newPurchaseProvider.Cant : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@cost", newPurchaseProvider.Cost != null ? (object)newPurchaseProvider.Cost : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@total", newPurchaseProvider.Total != null ? (object)newPurchaseProvider.Total : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@productName", string.IsNullOrEmpty(newPurchaseProvider.ProductName) ? DBNull.Value : newPurchaseProvider.ProductName));
                        command.Parameters.Add(new SqlParameter("@providerName", string.IsNullOrEmpty(newPurchaseProvider.ProviderName) ? DBNull.Value : newPurchaseProvider.ProviderName));
                        command.Parameters.Add(new SqlParameter("@businessName", string.IsNullOrEmpty(newPurchaseProvider.BusinessName) ? DBNull.Value : newPurchaseProvider.BusinessName));
                        command.Parameters.Add(new SqlParameter("@newPrice", newPurchaseProvider.Price != null ? (object)newPurchaseProvider.Price : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@operation", 1));

                        connection.Open();
                        command.ExecuteNonQuery();
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
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProvider", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
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

        //------------------------------------------------------------------------------------
        //                              GetById                                             
        //------------------------------------------------------------------------------------
        public Provider GetById(int id)
        {
            Provider provider = null;
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idProvider", id),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@description", DBNull.Value),
                            new SqlParameter("@operation", '2')
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                provider = new Provider
                                {
                                    Id = Convert.ToInt32(dataReader["idProvider"]),
                                    Name = dataReader["name"].ToString(),
                                    PhoneNumber = dataReader["phoneNumber"].ToString(),
                                    Description = dataReader["description"].ToString(),

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomDataException("An error occurred while fetching the type payment by ID.", ex);
            }

            return provider;
        }

        //------------------------------------------------------------------------------------
        //                              Update                                             
        //------------------------------------------------------------------------------------
        public OperationResult Update(Provider newProvider)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProviderCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@idProvider", newProvider.Id != 0 ? (object)newProvider.Id : DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(newProvider.Name) ? DBNull.Value : newProvider.Name));
                        command.Parameters.Add(new SqlParameter("@phoneNumber", string.IsNullOrEmpty(newProvider.PhoneNumber) ? DBNull.Value : newProvider.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(newProvider.Description) ? DBNull.Value : newProvider.Description));
                        command.Parameters.Add(new SqlParameter("@operation", 3));

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                string userMessage = "An error occurred while processing your request.";

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
