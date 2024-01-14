using Sistema_Inventario_Manitos_Maravillosas.Areas.Admin.Models;
using Sistema_Inventario_Manitos_Maravillosas.Models;
using System.Data;
using System.Data.SqlClient;
using System;
using Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Models;

namespace Sistema_Inventario_Manitos_Maravillosas.Areas.Facturation.Data.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(string id);
        OperationResult Add(Product newProduct);
        OperationResult Update(Product product);
        OperationResult Delete(string id);

        Product GetStockById(string id, int quantity);

        OperationResult AddStock(string id, int quantity);

        OperationResult UpdateStock(string id, int quantity);

    }
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;

        private OperationResult result = new OperationResult(true, "");

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public OperationResult Add(Product newProduct)
        {
            throw new NotImplementedException();
        }

        public OperationResult AddStock(string id, int quantity)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------------
        //                              GetAll                                             
        //------------------------------------------------------------------------------------
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string connectionString = _configuration.GetConnectionString("ConnectionToDataBase");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("spProductCRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new SqlParameter[]
                        {
                            new SqlParameter("@idClient", DBNull.Value),
                            new SqlParameter("@name", DBNull.Value),
                            new SqlParameter("@lastName1", DBNull.Value),
                            new SqlParameter("@lastName2", DBNull.Value),
                            new SqlParameter("@email", DBNull.Value),
                            new SqlParameter("@phoneNumber", DBNull.Value),
                            new SqlParameter("@operation", '2') // Operation for 'Read' is 2
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
                                Product product = new Product();

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Handle the exception as per your application's policy
                products.Clear(); // This will return an empty list in case of an error.
            }

            return products;
        }

        public Product GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Product GetStockById(string id, int quantity)
        {
            Product product = new Product();
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
                    throw new ApplicationException("Error executing SQL command: " + sqlEx.Message, sqlEx);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Consider whether to throw a custom exception or handle it differently
                throw new ApplicationException("An error occurred: " + ex.Message, ex);
            }

            return product;
        }

        public OperationResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateStock(string id, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
